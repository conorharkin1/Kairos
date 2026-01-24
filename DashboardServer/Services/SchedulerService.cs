using DashboardServer.Models;
using DashboardServer.Models.Constraints;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Hangfire;

namespace DashboardServer.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IUserResolverService _userResolverService;
        private readonly ISchedulerRepository _schedulerRepository;
        public SchedulerService(IUserResolverService userResolverService, ISchedulerRepository schedulerRepository)
        {
            _userResolverService = userResolverService;
            _schedulerRepository = schedulerRepository;
        }
        public void RunScheduler()
        {
            string userId = _userResolverService.UserId!;
            BackgroundJob.Enqueue(() => BackgroundRunScheduler(userId));
        }

        public async Task BackgroundRunScheduler(string userId)
        {
            SchedulerData data = await _schedulerRepository.GetSchedulerData(userId);

            if (data.Assignments.Count == 0)
                return;

            if (data.DailyCapacities.Count == 0 || data.CourseWeights.Count == 0 || data.Rules == null)
                return;

            // Step 1: Calculate the total available working hours of the week from DailyCapacities constraint
            int totalAvailableHours = CalculateTotalHours(data.DailyCapacities);
            
            // Step 2: Calculate the total hours each course gets allocated based on the course weights constraint
            Dictionary<int, double> courseHoursDict = CalculateHoursPerCourse(totalAvailableHours, data.CourseWeights);
            
            // Step 3: Calculate an urgency score for each assignment based on its proximity to deadline
            (double totalUrgency,  Dictionary<int, double> assignmentUrgencyDict) = CalculateUrgency(data.Assignments);
            
            // Step 4: Calculate the hours each assignment gets allocated based on it's urgency and the hours the course has available
            Dictionary<int, double> hoursPerAssignment = CalculateHoursPerAssignment(data.Assignments, courseHoursDict, totalUrgency, assignmentUrgencyDict);

            // Step 5: Convert those hoursPerAssignment into blocks based on scheduler rules constraint. Each block contains a priority and assignmentId
            int minBlockLength = data.Rules.MinSessionLength;
            List<ScheduleBlock> blocks = ConstructScheduleBlocks(courseHoursDict, hoursPerAssignment, minBlockLength, data.Assignments, totalAvailableHours);

            // Step 6: Calculate the priority of each block. Priority = (assignmentUrgency * 0.7) + (courseWeight * 0.3) where 0.7 and 0.3 are the behavioural constraints. (increase 0.7 to prioritise deadlines, increase 0.3 to prioritise courseweights)
            //CalculateBlockPriority(data.Assignments, blocks, assignmentUrgencies, data.CourseWeights);
            
            // Step 7: Sort the blocks by highest priority. Add them to a queue.
            //var sortedBlocks = blocks.OrderByDescending(b => b.Priority).ToList();
            //Queue<ScheduleBlock> blockQueue = new Queue<ScheduleBlock>(sortedBlocks);

            // Step 8: Starting on the day after the current day, figure out the available blocks based on the daily cap and push from the queue the required blocks.
            //DateTime currentDate = DateTime.UtcNow.Date.AddDays(1);
        }

        private static int CalculateTotalHours(List<DailyCapacity> constraints)
        {
            var constraintHours = constraints.Select(c => c.MaxHours);
            
            int total = 0;
            foreach(int hours in constraintHours)
            {
                total += hours;
            }
            return total;
        }

        private static Dictionary<int, double> CalculateHoursPerCourse(int totalAvailableHours, List<CourseWeight> constraints)
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
                double roundedHours = Math.Floor(hours / 0.5) * 0.5 ; // Rounds down to the nearest 0.5. Eg. 5.8 -> 5.5 
                results.Add(con.CourseId, roundedHours);
            }

            return results;
        }

        private static (double, Dictionary<int, double>) CalculateUrgency(List<Assignment> assignments)
        {
            double totalUrgency = 0;
            Dictionary<int, double> assignmentUrgencies = new Dictionary<int, double>();

            foreach(var assignment in assignments)
            {
                if (assignment.DueDate == null)
                    continue;
                double urgency = CalculateAssignmentUrgency((DateTime)assignment.DueDate);
                totalUrgency += urgency;

                assignmentUrgencies.Add(assignment.Id, urgency);
            }

            return (totalUrgency, assignmentUrgencies);

        }
        private static double CalculateAssignmentUrgency(DateTime dueDate)
        {
            TimeSpan diff = dueDate - DateTime.UtcNow;
            double daysOut = diff.TotalDays;

            if (daysOut <= 3)
            {
                return 3;
            } 

            return 3 / daysOut;
        }

        private static Dictionary<int, double> CalculateHoursPerAssignment(List<Assignment> assignments, Dictionary<int, double> courseHoursDict, double totalUrgency, Dictionary<int, double> assignmentUrgencies)
        {
            Dictionary<int, double> assignmentHoursDict = new Dictionary<int, double>();
            foreach(var assignment in assignments)
            {
                double courseHours = courseHoursDict[assignment.CourseId];
                double urgency = assignmentUrgencies[assignment.Id];

                double allocatedAssignmentHours = courseHours * (urgency / totalUrgency);
                double roundedHours = Math.Floor(allocatedAssignmentHours / 0.5) * 0.5; // Rounds down to the nearest 0.5. Eg. 5.8 -> 5.5 

                assignmentHoursDict.Add(assignment.Id, roundedHours);
            }

            return assignmentHoursDict;
        }

        private static List<ScheduleBlock> ConstructScheduleBlocks(double hoursToAllocate, int minBlockLength, int courseId, int assignmentId)
        {
            List<ScheduleBlock> blocks = new List<ScheduleBlock>();
            var minutesToAllocate = hoursToAllocate * 60;
            var numBlocksToCreate = Math.Floor(minutesToAllocate / minBlockLength);

            for(int i = 0; i < numBlocksToCreate; i++)
            {
                blocks.Add(new ScheduleBlock {
                    AssignmentId = assignmentId,
                    CourseId = courseId,
                    Priority = 0,
                    BlockType = assignmentId != -1 ? BlockType.Assignment : BlockType.Study
                });
            }

            return blocks;
        }

        private static List<ScheduleBlock> ConstructScheduleBlocks(Dictionary<int, double> courseHoursDict, Dictionary<int, double> hoursPerAssignment, int minBlockLength, List<Assignment> assignments, int maxAvailableHours)
        {
            List<ScheduleBlock> blocks = new List<ScheduleBlock>();

            foreach(KeyValuePair<int, double> entry in courseHoursDict)
            {
                int courseId = entry.Key;
                double courseHoursRemainingToAlloc = entry.Value;

                if (courseHoursRemainingToAlloc == 0) { continue; }

                foreach(var assignment in assignments)
                {
                    if(assignment.CourseId == courseId)
                    {
                        double allocatedAssignmentHours = hoursPerAssignment[assignment.Id];
                        blocks.AddRange(ConstructScheduleBlocks(allocatedAssignmentHours, minBlockLength, courseId, assignment.Id));
                        courseHoursRemainingToAlloc -= allocatedAssignmentHours;
                    }
                }

                blocks.AddRange(ConstructScheduleBlocks(courseHoursRemainingToAlloc, minBlockLength, courseId, -1));
            }

            return blocks;
        }

        private static void CalculateBlockPriority(List<Assignment> assignments, List<ScheduleBlock> blocks, Dictionary<int, double> assignmentUrgencies, List<CourseWeight> courseWeights)
        {
            // Formula -> Priority = (assignmentUrgency * 0.7) + (courseWeight * 0.3)
            foreach (var assignment in assignments)
            {
                double urgency = assignmentUrgencies[assignment.Id];
                double courseWeight = courseWeights.First(cw => cw.CourseId == assignment.CourseId).Weight;

                double priority = (urgency * 0.7) + (courseWeight * 0.3);

                foreach(var block in blocks.Where(b => b.AssignmentId == assignment.Id))
                {
                    block.Priority = priority;
                }
            }
        }
    }
}