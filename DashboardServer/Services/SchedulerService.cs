using AutoMapper;
using DashboardServer.DTOs.ModelDTOs;
using DashboardServer.Exceptions;
using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;

namespace DashboardServer.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IUserResolverService _userResolverService;
        private readonly ISchedulerRepository _schedulerRepository;
        private readonly IMapper _mapper;
        public SchedulerService(IUserResolverService userResolverService, ISchedulerRepository schedulerRepository, IMapper mapper)
        {
            _userResolverService = userResolverService;
            _schedulerRepository = schedulerRepository;
            _mapper = mapper;
        }

        public async Task<ScheduleDTO?> GetSchedule(string userId)
        {
            var schedule = await _schedulerRepository.GetSchedule(userId);
            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task RunScheduler()
        {
            string userId = _userResolverService.UserId!;
            await ExecuteScheduler(userId);
        }

        private async Task ExecuteScheduler(string userId)
        {
            SchedulerData data = await _schedulerRepository.GetDataForScheduler(userId);

            if (data.Assignments == null || data.Assignments.Count == 0)
                return;

            if (data.DailyCapacities == null || data.DailyCapacities.Count == 0 || data.CourseWeights == null || data.CourseWeights.Count == 0 || data.Rules == null)
                return;

            // Step 1: Calculate the total available working hours of the week from DailyCapacities constraint
            int totalAvailableHours = await CalculateTotalHours(data.DailyCapacities);
            int minBlockLength = data.Rules.MinSessionLength;
            int maxBlockLength = data.Rules.MaxSessionLength;

            // Step 2: Calculate the total hours each course gets allocated based on the course weights constraint
            Dictionary<int, double> courseHoursDict = await CalculateHoursPerCourse(totalAvailableHours, data.CourseWeights);

            // Step 3: Calculate an urgency score for each assignment based on its proximity to deadline
            (Dictionary<int, double> courseTotalUrgency, Dictionary<int, double> assignmentUrgencyDict) = await CalculateUrgency(data.Assignments);

            // Step 4: Calculate the hours each assignment gets allocated based on it's urgency and the hours the course has available
            Dictionary<int, double> hoursPerAssignment = await CalculateHoursPerAssignment(data.Assignments, courseHoursDict, courseTotalUrgency, assignmentUrgencyDict);

            // Step 5: Convert those hoursPerAssignment into blocks based on scheduler rules constraint. Each block contains a priority and assignmentId
            List<ScheduleBlock> blocks = await ConstructScheduleBlocks(courseHoursDict, hoursPerAssignment, minBlockLength, data.Assignments, userId);

            //Step 6: Calculate the priority of each block. Priority = (assignmentUrgency * 0.8) + (courseWeight * 0.2) where 0.8 and 0.2 are the behavioural constraints. (increase 0.8 to prioritise deadlines, increase 0.2 to prioritise courseweights)
            // Returns blocks sorted by priority from highest to lowest
            blocks = await CalculateBlockPriority(data.Assignments, blocks, assignmentUrgencyDict, data.CourseWeights);

            // initialise the schedule matrix with avoid periods constraint and max block length constraint
            int[,] initialMatrix = await ConstructInitialScheduleMatrix(maxBlockLength, data.AvoidPeriods ?? []);

            //Step 7: Sort the blocks by highest priority. Add them to a queue.
            Queue<ScheduleBlock> blockQueue = new Queue<ScheduleBlock>(blocks);

            //Step 8: Build the schedule matrix from the block queue and daily capacities constraint
            ScheduleBlock?[,] schedule = await BuildSchedule(initialMatrix, data.DailyCapacities, blockQueue);

            //Step 9: Save the schedule to the database
            await _schedulerRepository.SaveSchedule(userId, schedule);
        }

        private static async Task<int> CalculateTotalHours(List<DailyCapacity> constraints)
        {
            var constraintHours = constraints.Select(c => c.MaxHours);

            int total = 0;
            foreach (int hours in constraintHours)
            {
                total += hours;
            }
            return total;
        }

        private static async Task<Dictionary<int, double>> CalculateHoursPerCourse(int totalAvailableHours, List<CourseWeight> constraints)
        {
            int totalWeight = 0;
            Dictionary<int, double> results = [];

            foreach (var weight in constraints.Select(w => w.Weight))
            {
                totalWeight += weight;
            }

            foreach (var con in constraints)
            {
                double hours = totalAvailableHours * ((double)con.Weight / totalWeight);
                double roundedHours = Math.Floor(hours / 0.5) * 0.5; // Rounds down to the nearest 0.5. Eg. 5.8 -> 5.5 
                results.Add(con.CourseId, roundedHours);
            }

            return results;
        }

        private static async Task<(Dictionary<int, double>, Dictionary<int, double>)> CalculateUrgency(List<Assignment> assignments)
        {
            // courseId, TotalUrgencyOfAssignmentsOnCourse
            Dictionary<int, double> courseTotalUrgency = new Dictionary<int, double>();
            // assignmentId, Urgency
            Dictionary<int, double> assignmentUrgency = new Dictionary<int, double>();

            foreach (var assignment in assignments)
            {
                if (assignment.DueDate == null)
                    continue;
                double urgency = await CalculateAssignmentUrgency((DateTime)assignment.DueDate);

                if (!courseTotalUrgency.ContainsKey(assignment.CourseId))
                {
                    courseTotalUrgency[assignment.CourseId] = 0;
                }
                courseTotalUrgency[assignment.CourseId] += urgency;

                assignmentUrgency.Add(assignment.Id, urgency);
            }

            return (courseTotalUrgency, assignmentUrgency);

        }
        private static async Task<double> CalculateAssignmentUrgency(DateTime dueDate)
        {
            // max urgency is 1, further away due date -> lower urgency
            double daysOut = (dueDate - DateTime.UtcNow).TotalDays;

            if (daysOut <= 1)
            {
                return 1;
            }
            else if (daysOut <= 3)
            {
                return 0.9;
            }
            else if (daysOut <= 7)
            {
                return 3 / daysOut;
            }

            return 2 / daysOut;
        }

        private static async Task<Dictionary<int, double>> CalculateHoursPerAssignment(List<Assignment> assignments, Dictionary<int, double> courseHoursDict, Dictionary<int, double> courseTotalUrgency, Dictionary<int, double> assignmentUrgencies)
        {
            Dictionary<int, double> assignmentHoursDict = new Dictionary<int, double>();
            foreach (var assignment in assignments)
            {
                double courseHours = courseHoursDict[assignment.CourseId];
                double courseUrgencyTotal = courseTotalUrgency[assignment.CourseId];
                double urgency = assignmentUrgencies[assignment.Id];

                // cap the allocatable assignment time to 80% of the courses time, this ensure some time is reserved for studying blocks
                double allocatableHoursForAssignments = courseHours * 0.8;

                double allocatedAssignmentHours = allocatableHoursForAssignments * (urgency / courseUrgencyTotal);
                double roundedHours = Math.Floor(allocatedAssignmentHours / 0.5) * 0.5; // Rounds down to the nearest 0.5. Eg. 5.8 -> 5.5 

                assignmentHoursDict.Add(assignment.Id, roundedHours);
            }

            return assignmentHoursDict;
        }

        private static async Task<List<ScheduleBlock>> CreateBlocks(double hoursToAllocate, int minBlockLength, int courseId, int? assignmentId, string assignmentName, string userId)
        {
            List<ScheduleBlock> blocks = new List<ScheduleBlock>();
            var minutesToAllocate = hoursToAllocate * 60;
            var numBlocksToCreate = Math.Floor(minutesToAllocate / minBlockLength);

            for (int i = 0; i < numBlocksToCreate; i++)
            {
                blocks.Add(new ScheduleBlock
                {
                    UserId = userId,
                    AssignmentId = assignmentId,
                    AssignmentName = assignmentName,
                    CourseId = courseId,
                    Priority = 0,
                    BlockType = assignmentId != -1 ? Models.BlockType.Assignment : Models.BlockType.Study
                });
            }

            return blocks;
        }

        private static async Task<List<ScheduleBlock>> ConstructScheduleBlocks(Dictionary<int, double> courseHoursDict, Dictionary<int, double> hoursPerAssignment, int minBlockLength, List<Assignment> assignments, string userId)
        {
            // loop through coursesHours pairs, construct assignment schedule blocks for the assignments and track the remainder of course time
            // create study schedule blocks from the remaining time. 
            List<ScheduleBlock> blocks = new List<ScheduleBlock>();

            foreach (KeyValuePair<int, double> entry in courseHoursDict)
            {
                if (entry.Value == 0) { continue; }

                double courseHoursRemainingToAlloc = entry.Value;
                int courseId = entry.Key;

                foreach (var assignment in assignments)
                {
                    if (assignment.CourseId == courseId)
                    {
                        double allocatedAssignmentHours = hoursPerAssignment[assignment.Id];
                        blocks.AddRange(await CreateBlocks(allocatedAssignmentHours, minBlockLength, courseId, assignment.Id, assignment.Name ?? "Assignment", userId));
                        courseHoursRemainingToAlloc -= allocatedAssignmentHours;
                    }
                }

                blocks.AddRange(await CreateBlocks(courseHoursRemainingToAlloc, minBlockLength, courseId, null, "Study", userId));
            }

            return blocks;
        }

        private static async Task<List<ScheduleBlock>> CalculateBlockPriority(List<Assignment> assignments, List<ScheduleBlock> blocks, Dictionary<int, double> assignmentUrgencies, List<CourseWeight> courseWeights)
        {
            // Normalise course weights
            Dictionary<int, double> normalisedCourseWeights = new Dictionary<int, double>();
            double totalWeight = courseWeights.Sum(cw => cw.Weight);
            foreach (var cw in courseWeights)
            {
                normalisedCourseWeights[cw.CourseId] = (double)cw.Weight / totalWeight;
            }

            // Formula -> Priority = (assignmentUrgency * 0.8) + (courseWeight * 0.2)
            foreach (var assignment in assignments)
            {
                double urgency = assignmentUrgencies[assignment.Id];
                double courseWeight = normalisedCourseWeights[assignment.CourseId];

                double priority = (urgency * 0.7) + (courseWeight * 0.3);

                foreach (var block in blocks.Where(b => b.AssignmentId == assignment.Id))
                {
                    block.Priority = priority;
                    priority -= 0.01;
                }
            }

            blocks = blocks.OrderByDescending(b => b.Priority).ToList();

            return blocks;
        }

        private static async Task<int[,]> ConstructInitialScheduleMatrix(int maxBlockLength, List<AvoidPeriod> avoidPeriodConstraints)
        {
            int rows = 24;  // 9am to 9pm
            int cols = 7;   // days of week
            int[,] matrix = new int[rows, cols];

            int maxBlocksInRow = maxBlockLength / 30;

            var avoidDict = avoidPeriodConstraints.ToDictionary(ap => ap.DayIndex, ap => ap.AvoidTimeBlocks);
            
            // iterating through days of week
            for (int day = 0; day < cols; day++)
            {
                int currentBlocksInRow = 0;

                bool[] isBadSlot = new bool[rows];

                if (avoidDict.TryGetValue(day, out var avoidTimeBlocks))
                {
                    foreach (var block in avoidTimeBlocks)
                    {
                        // eg. StartTime = 11am -> corresponds to slot at index 2 of matrix
                        int startIndex = (block.StartTime.Hours - 9) * 2 + (block.StartTime.Minutes == 30 ? 1 : 0);
                        int endIndex = (block.EndTime.Hours - 9) * 2 + (block.EndTime.Minutes == 30 ? 1 : 0);

                        for (int i = startIndex; i < endIndex; i++)
                            isBadSlot[i] = true;
                    }
                }

                // iterating through times of day
                for (int slot = 0; slot < rows; slot++)
                {
                    if (isBadSlot[slot] || currentBlocksInRow == maxBlocksInRow)
                    {
                        matrix[slot, day] = -1;
                        currentBlocksInRow = 0;
                    }
                    else
                    {
                        matrix[slot, day] = 1;
                        currentBlocksInRow++;
                    }
                }
            }

            return matrix;

        }

        private static async Task<ScheduleBlock?[,]> BuildSchedule(int[,] initialMatrix, List<DailyCapacity> dailyCapacities, Queue<ScheduleBlock> blockQueue)
        {
            int rows = 24;  // 9am to 9pm
            int cols = 7;   // days of week
            ScheduleBlock?[,] schedule = new ScheduleBlock[rows, cols];

            DateTime startDate = DateTime.UtcNow.Date;
            int startDayIndex = (int)startDate.DayOfWeek;

            for (int day = 0; day < cols; day++)
            {
                int dayIndex = (startDayIndex + day) % 7;
                int maxAllocatableHours = dailyCapacities.First(dc => dc.DayIndex == dayIndex).MaxHours;

                if (maxAllocatableHours == 0)
                    continue;

                int slotsToAllocate = maxAllocatableHours * 2; // each slot is 30 mins

                for (int slot = 0; slot < rows; slot++)
                {
                    if (slotsToAllocate == 0)
                        break;
                    if (blockQueue.Count == 0)
                        break;


                    // if bad slot
                    if (initialMatrix[slot, dayIndex] == -1)
                    {
                        schedule[slot, dayIndex] = null;
                    }
                    else
                    {
                        var block = blockQueue.Dequeue();
                        DateTime startTime = startDate.AddDays(day).AddHours(9).AddMinutes(slot * 30);
                        block.StartTime = startTime;
                        block.EndTime = startTime.AddMinutes(30);

                        schedule[slot, dayIndex] = block;
                        slotsToAllocate--;
                    }
                }
            }

            return schedule;
        }
    }
}