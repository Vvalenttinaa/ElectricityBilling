namespace ElectricityBilling.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendRecommendationAsync(
            string recipientEmail,
            string planName,
            decimal monthlyConsumption,
            decimal estimatedCost);
    }
}