namespace ElectricityBilling.Domain.Entities
{
    public class Plan
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Discount { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<PricingTier> PricingTiers { get; set; }
            = new List<PricingTier>();
    }
}
