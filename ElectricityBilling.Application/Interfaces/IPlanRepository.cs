using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Interfaces
{
    public interface IPlanRepository
    {
        Task<List<Plan>> GetActivePlansWithPricingTiersAsync();
        Task<List<Plan>> GetAllAsync();
        Task<Plan> AddAsync(Plan plan);
        Task<Plan?> GetByIdForUpdateAsync(Guid id);
        Task SaveChangesAsync();
        void RemovePricingTiers(IEnumerable<PricingTier> tiers);
        Task<Plan?> GetByIdAsync(Guid id);

    }
}
