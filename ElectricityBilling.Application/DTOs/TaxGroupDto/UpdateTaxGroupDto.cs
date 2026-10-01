using System.ComponentModel.DataAnnotations;

namespace ElectricityBilling.Application.DTOs.TaxGroupDto
{
    public class UpdateTaxGroupDto
    {
        [Required(ErrorMessage = "Tax group name is required.")]
        [MaxLength(100, ErrorMessage = "Tax group name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        [Range(0, 1, ErrorMessage = "VAT must be between 0 and 1.")]
        public decimal Vat { get; set; }

        [Range(0, 1, ErrorMessage = "Eco tax must be between 0 and 1.")]
        public decimal EcoTax { get; set; }
    }
}