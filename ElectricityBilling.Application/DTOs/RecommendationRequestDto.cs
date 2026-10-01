using System.ComponentModel.DataAnnotations;

namespace ElectricityBilling.Application.DTOs
{
    public class RecommendationRequestDto

    {
        [Range(0.01, double.MaxValue)]
        public decimal MonthlyConsumption { get; set; }

        public Guid TaxGroupId { get; set; }

        [EmailAddress(ErrorMessage = "Wrong format.")]
        public string? Email { get; set; }

        public bool SendEmail { get; set; }

    }
}
