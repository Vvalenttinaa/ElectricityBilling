using System.ComponentModel.DataAnnotations;

namespace ElectricityBilling.Application.DTOs
{
    public class UpdatePlanDto
    {
        [Required(ErrorMessage = "Plan name is required.")]
        [MaxLength(100, ErrorMessage = "Plan name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1.")]
        public decimal Discount { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Pricing tiers are required.")]
        [MinLength(1, ErrorMessage = "Plan must contain at least one pricing tier.")]
        public List<CreatePricingTierDto> PricingTiers { get; set; } = new();
    }
}