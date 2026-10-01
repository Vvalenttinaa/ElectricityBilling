using ElectricityBilling.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ElectricityBilling.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendRecommendationAsync(
            string recipientEmail,
            string planName,
            decimal monthlyConsumption,
            decimal estimatedCost)
        {
            var message = new MimeMessage();

            var fromName =
                _configuration["EmailSettings:FromName"];

            var fromEmail =
                _configuration["EmailSettings:FromEmail"];

            message.From.Add(
                new MailboxAddress(
                    fromName,
                    fromEmail));

            message.To.Add(
                MailboxAddress.Parse(recipientEmail));

            message.Subject =
                "Your electricity plan recommendation";

            message.Body = new TextPart("html")
            {
                Text = $"""
                    <h2>Electricity Plan Recommendation</h2>

                    <p>
                        Based on your monthly electricity consumption,
                        we recommend the following plan:
                    </p>

                    <h3>{planName}</h3>

                    <p>
                        Monthly consumption:
                        <strong>{monthlyConsumption:N2} kWh</strong>
                    </p>

                    <p>
                        Estimated monthly cost:
                        <strong>{estimatedCost:N2}</strong>
                    </p>

                    <p>
                        Thank you for using Electricity Billing.
                    </p>
                    """
            };

            using var smtp = new SmtpClient();

            smtp.CheckCertificateRevocation = false;

            var host =
                _configuration["EmailSettings:SmtpHost"];

            var port =
                int.Parse(
                    _configuration["EmailSettings:SmtpPort"]!);

            try
            {
                var username =
                  _configuration["EmailSettings:Username"];

                var password =
                    _configuration["EmailSettings:Password"];

                Console.WriteLine($"SMTP host: {host}");
                Console.WriteLine($"SMTP port: {port}");
                Console.WriteLine($"SMTP username: {username}");
                Console.WriteLine(
                    $"Password configured: {!string.IsNullOrWhiteSpace(password)}");



                await smtp.ConnectAsync(
                    host,
                    port,
                    MailKit.Security.SecureSocketOptions.StartTls);

                Console.WriteLine($"SMTP host: {host}");
                Console.WriteLine($"SMTP username: {username}");
                Console.WriteLine($"Password configured: {!string.IsNullOrWhiteSpace(password)}");

                if (!string.IsNullOrWhiteSpace(username))
                {
                    await smtp.AuthenticateAsync(
                        username,
                        password);
                }

                await smtp.SendAsync(message);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}