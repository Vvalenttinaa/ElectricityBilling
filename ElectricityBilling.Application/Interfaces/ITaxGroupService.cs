using ElectricityBilling.Application.DTOs.TaxGroupDto;

namespace ElectricityBilling.Application.Interfaces;

public interface ITaxGroupService
{
    Task<List<TaxGroupDto>> GetAllAsync();

    Task<TaxGroupDto> CreateAsync(CreateTaxGroupDto dto);

    Task<TaxGroupDto?> UpdateAsync(
        Guid id,
        UpdateTaxGroupDto dto);

    Task<bool> DeleteAsync(Guid id);
    Task<TaxGroupDto> GetByIdAsync(Guid id);
}