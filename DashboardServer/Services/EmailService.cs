using System.Text;
using DashboardServer.Models;
using DashboardServer.Repositories.Interfaces;
using DashboardServer.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Resend;

namespace DashboardServer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly ILogger<EmailService> _logger;

        private const string FromAddress = "Student Scheduler <onboarding@resend.dev>";

        public EmailService(
            IResend resend,
            IUserRepository userRepository,
            IAssignmentRepository assignmentRepository,
            ILogger<EmailService> logger)
        {
            _resend = resend;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
            _logger = logger;
        }

        public async Task SendReminderEmails()
        {
            var today = DateTime.UtcNow.Date;
            var users = await _userRepository.GetAllOptedInForEmailsAsync();
            var allAssignments = await _assignmentRepository.GetAllUpcomingUnsubmittedAsync();

            var assignmentsByUserId = allAssignments
                .GroupBy(a => a.UserId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var user in users)
            {
                try
                {
                    if (!assignmentsByUserId.TryGetValue(user.Id, out var assignments) || assignments.Count == 0)
                        continue;

                    var htmlBody = BuildHtmlEmailBody(user, assignments, today);

                    var message = new EmailMessage();
                    message.From = FromAddress;
                    message.To.Add(user.Email!);
                    message.Subject = "Student Scheduler - Upcoming Assignment Reminders";
                    message.HtmlBody = htmlBody;

                    await _resend.EmailSendAsync(message);

                    _logger.LogInformation(
                        "Reminder email sent to {Email} with {Count} upcoming assignments",
                        user.Email, assignments.Count);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Failed to send reminder email to user {UserId} ({Email})",
                        user.Id, user.Email);
                }
            }
        }

        private static string BuildHtmlEmailBody(
            ApplicationUser user,
            List<Assignment> assignments,
            DateTime today)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<html><body style=\"font-family: Arial, sans-serif; color: #333;\">");
            sb.AppendLine("<h2>Upcoming Assignment Reminders</h2>");
            sb.AppendLine($"<p>Hi {user.FirstName ?? "Student"},</p>");
            sb.AppendLine("<p>You have the following upcoming assignments:</p>");

            sb.AppendLine("<table style=\"border-collapse: collapse; width: 100%; max-width: 600px;\">");
            sb.AppendLine("<thead><tr style=\"background-color: #f4f4f4;\">");
            sb.AppendLine("<th style=\"text-align: left; padding: 8px; border: 1px solid #ddd;\">Assignment</th>");
            sb.AppendLine("<th style=\"text-align: left; padding: 8px; border: 1px solid #ddd;\">Course</th>");
            sb.AppendLine("<th style=\"text-align: left; padding: 8px; border: 1px solid #ddd;\">Due</th>");
            sb.AppendLine("</tr></thead><tbody>");

            foreach (var assignment in assignments)
            {
                var daysUntilDue = (assignment.DueDate!.Value.Date - today).Days;
                string dueText;
                string dueStyle;

                if (daysUntilDue == 0)
                {
                    dueText = "Due today!";
                    dueStyle = "padding: 8px; border: 1px solid #ddd; color: #e74c3c; font-weight: bold;";
                }
                else
                {
                    dueText = $"Due in {daysUntilDue} day{(daysUntilDue != 1 ? "s" : "")}";
                    dueStyle = "padding: 8px; border: 1px solid #ddd;";
                }

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td style=\"padding: 8px; border: 1px solid #ddd;\">{assignment.Name}</td>");
                sb.AppendLine($"<td style=\"padding: 8px; border: 1px solid #ddd;\">{assignment.Course?.Name}</td>");
                sb.AppendLine($"<td style=\"{dueStyle}\">{dueText}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<b style=\"margin-top: 16px;\">GET TO WORK</b>");
            sb.AppendLine("<hr style=\"border: none; border-top: 1px solid #eee;\" />");
            sb.AppendLine("<p style=\"font-size: 12px; color: #999;\">");
            sb.AppendLine("You are receiving this email because you have an account with Student Scheduler. ");
            sb.AppendLine("To stop receiving these emails, update your notification preferences in your account settings.");
            sb.AppendLine("</p>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }
    }
}
