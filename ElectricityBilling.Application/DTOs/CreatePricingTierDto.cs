using System.ComponentModel.DataAnnotations;

namespace ElectricityBilling.Application.DTOs
{
    public class CreatePricingTierDto
    {
        [Range(0, double.MaxValue,
            ErrorMessage = "Threshold cannot be negative.")]
        public decimal? Threshold { get; set; }

        [Range(0.000001, double.MaxValue,
            ErrorMessage = "Price per kWh must be greater than 0.")]
        public decimal PricePerKwh { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Sort order must be greater than 0.")]
        public int SortOrder { get; set; }
    }
}