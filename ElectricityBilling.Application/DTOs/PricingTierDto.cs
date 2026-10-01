namespace ElectricityBilling.Application.DTOs
{
    public class PricingTierDto
    {

        public Guid Id { get; set; }

        public decimal? Threshold { get; set; }

        public decimal PricePerKwh { get; set; }

        public int SortOrder { get; set; }

    }
}
