namespace ElectricityBilling.Domain.Entities
{
    public class TaxRate
    {
        public Guid Id { get; set; }

        public Guid TaxGroupId { get; set; }

        public decimal Vat { get; set; }

        public decimal EcoTax { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidTo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public TaxGroup TaxGroup { get; set; } = null!;
    }
}
