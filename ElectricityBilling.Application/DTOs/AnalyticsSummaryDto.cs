namespace ElectricityBilling.Application.DTOs
{
    public class AnalyticsSummaryDto
    {
        public int TotalRecommendations { get; set; }

        public decimal AverageConsumptionKwh { get; set; }

        public decimal AverageEstimatedCost { get; set; }

        public List<PlanSelectionTrendDto> PlanSelectionTrends { get; set; }
            = new();
    }
}