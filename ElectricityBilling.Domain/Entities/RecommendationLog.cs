namespace ElectricityBilling.Domain.Entities
{
    public class RecommendationLog
    {
        public Guid Id { get; set; }

        public decimal ConsumptionKwh { get; set; }

        public Guid TaxGroupId { get; set; }

        public Guid RecommendedPlanId { get; set; }

        public decimal EstimatedCost { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? SessionId { get; set; }

        public Plan RecommendedPlan { get; set; } = null!;

        public TaxGroup TaxGroup { get; set; } = null!;
    }
}