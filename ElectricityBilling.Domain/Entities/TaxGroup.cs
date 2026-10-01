namespace ElectricityBilling.Domain.Entities
{
    public class TaxGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<TaxRate> TaxRates { get; set; }
            = new List<TaxRate>();
    }
}
