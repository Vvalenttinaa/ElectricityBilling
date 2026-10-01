namespace ElectricityBilling.Application.DTOs
{
    public class PlanDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Discount { get; set; }

        public bool IsActive { get; set; }

        public List<PricingTierDto> PricingTiers { get; set; } = new();
    }
}
