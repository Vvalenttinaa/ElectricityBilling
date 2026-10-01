using ElectricityBilling.Application.DTOs.TaxGroupDto;
using ElectricityBilling.Application.Exceptions;
using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Services;

public class TaxGroupService : ITaxGroupService
{
    private readonly ITaxGroupRepository _taxGroupRepository;

    public TaxGroupService(
        ITaxGroupRepository taxGroupRepository)
    {
        _taxGroupRepository = taxGroupRepository;
    }

    public async Task<List<TaxGroupDto>> GetAllAsync()
    {
        var taxGroups =
            await _taxGroupRepository.GetAllAsync();

        return taxGroups
            .Select(MapToDto)
            .ToList();
    }

    public async Task<TaxGroupDto> CreateAsync(
        CreateTaxGroupDto dto)
    {

        var taxGroup = new TaxGroup
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            Description = dto.Description,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow,

            TaxRates = new List<TaxRate>
            {
                new TaxRate
                {
                    Id = Guid.NewGuid(),
                    Vat = dto.Vat,
                    EcoTax = dto.EcoTax,
                    IsActive = true,
                    ValidFrom = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            }
        };

        var created =
            await _taxGroupRepository.AddAsync(taxGroup);

        return MapToDto(created);
    }

    public async Task<TaxGroupDto> UpdateAsync(
        Guid id,
        UpdateTaxGroupDto dto)
    {

        var taxGroup =
            await _taxGroupRepository.GetByIdForUpdateAsync(id);

        if (taxGroup is null)
            throw new NotFoundException("TaxGroup not found."); ;

        taxGroup.Name = dto.Name.Trim();
        taxGroup.Description = dto.Description;
        taxGroup.IsActive = dto.IsActive;
        taxGroup.UpdatedAt = DateTime.UtcNow;

        var activeTaxRate = taxGroup.TaxRates
            .FirstOrDefault(tr => tr.IsActive);

        if (activeTaxRate is not null)
        {
            activeTaxRate.IsActive = false;
            activeTaxRate.ValidTo = DateTime.UtcNow;
            activeTaxRate.UpdatedAt = DateTime.UtcNow;
        }

        taxGroup.TaxRates.Add(new TaxRate
        {
            TaxGroupId = taxGroup.Id,
            Vat = dto.Vat,
            EcoTax = dto.EcoTax,
            IsActive = true,
            ValidFrom = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        });

        await _taxGroupRepository.SaveChangesAsync();

        return MapToDto(taxGroup);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var taxGroup =
            await _taxGroupRepository.GetByIdForUpdateAsync(id);

        if (taxGroup is null)
            return false;

        taxGroup.IsActive = false;
        taxGroup.UpdatedAt = DateTime.UtcNow;

        foreach (var taxRate in taxGroup.TaxRates)
        {
            if (!taxRate.IsActive)
                continue;

            taxRate.IsActive = false;
            taxRate.ValidTo = DateTime.UtcNow;
            taxRate.UpdatedAt = DateTime.UtcNow;
        }

        await _taxGroupRepository.SaveChangesAsync();

        return true;
    }

    private static TaxGroupDto MapToDto(
        TaxGroup taxGroup)
    {
        var activeTaxRate = taxGroup.TaxRates
            .Where(tr => tr.IsActive)
            .OrderByDescending(tr => tr.ValidFrom)
            .FirstOrDefault();

        return new TaxGroupDto
        {
            Id = taxGroup.Id,
            Name = taxGroup.Name,
            Description = taxGroup.Description,
            IsActive = taxGroup.IsActive,
            Vat = activeTaxRate?.Vat ?? 0,
            EcoTax = activeTaxRate?.EcoTax ?? 0
        };
    }


    public async Task<TaxGroupDto> GetByIdAsync(Guid id)
    {
        var taxGroup =
            await _taxGroupRepository.GetActiveByIdAsync(id);

        if (taxGroup is null)
            throw new NotFoundException("Tax group not found.");

        return MapToDto(taxGroup);
    }
}