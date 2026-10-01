using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Interfaces
{
    public interface ITaxGroupRepository
    {
        Task<TaxGroup?> GetByIdWithActiveTaxRateAsync(Guid id);

        Task<List<TaxGroup>> GetAllAsync();

        Task<TaxGroup?> GetByIdForUpdateAsync(Guid id);

        Task<TaxGroup> AddAsync(TaxGroup taxGroup);

        Task SaveChangesAsync();

        Task<TaxGroup?> GetActiveByIdAsync(Guid id);
    }
}
