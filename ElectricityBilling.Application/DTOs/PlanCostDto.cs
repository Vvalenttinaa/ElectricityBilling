namespace ElectricityBilling.Application.DTOs
{
    public class PlanCostDto
    {
        public Guid PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public decimal PricePerKwh { get; set; }

        public decimal BaseCost { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal VatAmount { get; set; }

        public decimal EcoTaxAmount { get; set; }

        public decimal TotalCost { get; set; }
    }
}
