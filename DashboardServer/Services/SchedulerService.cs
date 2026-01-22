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

        public async void BackgroundRunScheduler(string userId)
        {
            SchedulerData data = await _schedulerRepository.GetSchedulerData(userId);

            if (data.Assignments.Count == 0)
                return;

            if (data.DailyCapacities.Count == 0 || data.CourseWeights.Count == 0 || data.Rules == null)
                return;

            // Step 1: Calculate the total available hours based on the DailyCapacities constraint
            int maxAvailableHours = CalculateTotalHours(data.DailyCapacities);
            
            // Step 2: Calculate the total hours each course gets allocated based on CourseWeights
            Dictionary<int, double> maxHoursPerCourse = CalculateHoursPerCourse(maxAvailableHours, data.CourseWeights);
            
            // Step 3: Calculate an urgency score for each assignment based on its proximity to deadline
            (double totalUrgency,  Dictionary<int, double> assignmentUrgencies) = CalculateUrgency(data.Assignments);
            
            // Step 4: Calculate the hoursPerAssignment by doing hoursPerCourse * (urgency / sumUrgencies)
            Dictionary<int, double> hoursPerAssignment = CalculateHoursPerAssignment(data.Assignments, maxHoursPerCourse, totalUrgency, assignmentUrgencies);

            // Step 5: Convert those hoursPerAssignment into blocks based on scheduler rules constraint. Each block contains a priority and assignmentId
            int minBlockLength = data.Rules.MinSessionLength;
            List<ScheduleBlock> blocks = GetScheduleBlocks(hoursPerAssignment, minBlockLength);


            // Step 6: Calculate the priority of each block. Priority = (assignmentUrgency * 0.7) + (courseWeight * 0.3) where 0.7 and 0.3 are the behavioural constraints. (increase 0.7 to prioritise deadlines, increase 0.3 to prioritise courseweights)
            CalculateBlockPriority(data.Assignments, blocks, assignmentUrgencies, data.CourseWeights);
            
            // Step 7: Sort the blocks by highest priority. Add them to a queue.
            // Step 8: Starting on the day after the current day, figure out the available blocks based on the daily cap and push from the queue the required blocks.
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

        private static Dictionary<int, double> CalculateHoursPerCourse(int maxAvailableHours, List<CourseWeight> constraints)
        {
            int totalWeight = 0;
            Dictionary<int, double> results = new Dictionary<int, double>();

            foreach (var weight in constraints.Select(w => w.Weight))
            {
                totalWeight += weight;
            }

            foreach (var con in constraints)
            {
                double hours = maxAvailableHours * (con.Weight / totalWeight);
                results.Add(con.CourseId, hours);
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

            // If an assignment is due in 3 days or less - bump the urgency to 1
            if (daysOut <= 3)
            {
                return 1;
            }

            return 1 / daysOut;
        }

        private static Dictionary<int, double> CalculateHoursPerAssignment(List<Assignment> assignments, Dictionary<int, double> maxHoursPerCourse, double totalUrgency, Dictionary<int, double> assignmentUrgencies)
        {
            Dictionary<int, double> assignmentHoursDict = new Dictionary<int, double>();
            foreach(var assignment in assignments)
            {
                double courseHours = maxHoursPerCourse[assignment.CourseId];
                double urgency = assignmentUrgencies[assignment.Id];

                double allocatedAssignmentHours = courseHours * (urgency / totalUrgency);

                assignmentHoursDict.Add(assignment.Id, allocatedAssignmentHours);
            }

            return assignmentHoursDict;
        }
    
        private static List<ScheduleBlock> GetScheduleBlocks(Dictionary<int, double> assignmentHours, int minBlockLength)
        {
            List<ScheduleBlock> blocks = new List<ScheduleBlock>();
            foreach(KeyValuePair<int, double> entry in assignmentHours)
            {
                var minutes = entry.Value * 60;
                var numBlocks = minutes / minBlockLength;

                for(int i = 0; i < numBlocks; i++)
                {
                   blocks.Add(new ScheduleBlock {
                        AssignmentId = entry.Key,
                        Priority = 0
                    });
                }
            }

            return blocks;
        }

        private static void CalculateBlockPriority(List<Assignment> assignments, List<ScheduleBlock> blocks, Dictionary<int, double> assignmentUrgencies, List<CourseWeight> courseWeights)
        {
            //foreach ()
        }
    }
}