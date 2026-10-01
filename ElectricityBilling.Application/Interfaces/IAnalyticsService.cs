using ElectricityBilling.Application.DTOs;

namespace ElectricityBilling.Application.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsSummaryDto> GetSummaryAsync();
    }
}