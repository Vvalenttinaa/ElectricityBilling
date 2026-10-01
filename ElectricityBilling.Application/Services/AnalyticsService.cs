using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Interfaces;

namespace ElectricityBilling.Application.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IRecommendationLogRepository
            _recommendationLogRepository;

        public AnalyticsService(
            IRecommendationLogRepository recommendationLogRepository)
        {
            _recommendationLogRepository =
                recommendationLogRepository;
        }

        public async Task<AnalyticsSummaryDto> GetSummaryAsync()
        {
            var logs = await _recommendationLogRepository
                .GetAllWithDetailsAsync();

            if (logs.Count == 0)
            {
                return new AnalyticsSummaryDto();
            }

            var planTrends = logs
                .GroupBy(x => new
                {
                    x.RecommendedPlanId,
                    x.RecommendedPlan.Name
                })
                .Select(group =>
                {
                    var count = group.Count();

                    return new PlanSelectionTrendDto
                    {
                        PlanId = group.Key.RecommendedPlanId,
                        PlanName = group.Key.Name,
                        SelectionCount = count,

                        Percentage = Math.Round(
                            (decimal)count / logs.Count * 100,
                            2)
                    };
                })
                .OrderByDescending(x => x.SelectionCount)
                .ToList();

            return new AnalyticsSummaryDto
            {
                TotalRecommendations = logs.Count,

                AverageConsumptionKwh = Math.Round(
                    logs.Average(x => x.ConsumptionKwh),
                    2),

                AverageEstimatedCost = Math.Round(
                    logs.Average(x => x.EstimatedCost),
                    2),

                PlanSelectionTrends = planTrends
            };
        }
    }
}