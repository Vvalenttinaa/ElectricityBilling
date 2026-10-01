namespace ElectricityBilling.Application.DTOs.TaxGroupDto
{

    public class TaxGroupDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public decimal Vat { get; set; }

        public decimal EcoTax { get; set; }
    }
}
