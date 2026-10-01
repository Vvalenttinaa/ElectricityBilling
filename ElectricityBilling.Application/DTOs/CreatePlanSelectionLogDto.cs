using System.ComponentModel.DataAnnotations;

public class CreatePlanSelectionLogDto
{
    public Guid PlanId { get; set; }

    [Range(0.01, double.MaxValue,
        ErrorMessage = "Consumption must be greater than 0.")]
    public decimal? Consumption { get; set; }
}