namespace ElectricityBilling.Domain.Entities
{
    public class PricingTier
    {
        public Guid Id { get; set; }

        public Guid PlanId { get; set; }

        public decimal? Threshold { get; set; }

        public decimal PricePerKwh { get; set; }

        public int SortOrder { get; set; }

        public Plan Plan { get; set; } = null!;
    }
}
