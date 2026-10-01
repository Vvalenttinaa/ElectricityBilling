namespace ElectricityBilling.Application.DTOs
{
    public class RecommendationResponseDto
    {
        public PlanCostDto RecommendedPlan { get; set; } = null!;

        public List<PlanCostDto> AllPlans { get; set; } = new();
    }
}
