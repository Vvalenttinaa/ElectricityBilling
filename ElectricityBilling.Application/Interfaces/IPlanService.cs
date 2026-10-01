using ElectricityBilling.Application.DTOs;

namespace ElectricityBilling.Application.Interfaces
{
    public interface IPlanService
    {
        Task<List<PlanDto>> GetAllAsync();
        Task<PlanDto> CreateAsync(CreatePlanDto dto);
        Task<PlanDto?> UpdateAsync(Guid id, UpdatePlanDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PlanDto> GetByIdAsync(Guid id);
    }
}
