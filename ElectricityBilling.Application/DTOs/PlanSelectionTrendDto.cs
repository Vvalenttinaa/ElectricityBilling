namespace ElectricityBilling.Application.DTOs
{
    public class PlanSelectionTrendDto
    {
        public Guid PlanId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public int SelectionCount { get; set; }

        public decimal Percentage { get; set; }
    }
}
