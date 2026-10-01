using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;
using ElectricityBilling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectricityBilling.Infrastructure.Repositories;

public class TaxGroupRepository : ITaxGroupRepository
{
    private readonly ApplicationDbContext _context;

    public TaxGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaxGroup?> GetByIdWithActiveTaxRateAsync(Guid id)
    {
        return await _context.TaxGroups
            .AsNoTracking()
            .Include(tg => tg.TaxRates.Where(tr => tr.IsActive))
            .FirstOrDefaultAsync(tg =>
                tg.Id == id &&
                tg.IsActive);
    }

    public async Task<List<TaxGroup>> GetAllAsync()
    {
        return await _context.TaxGroups
            .AsNoTracking()
            .Where(tg => tg.IsActive)
            .Include(tg => tg.TaxRates.Where(tr => tr.IsActive))
            .OrderBy(tg => tg.Name)
            .ToListAsync();
    }

    public async Task<TaxGroup?> GetByIdForUpdateAsync(Guid id)
    {
        return await _context.TaxGroups
            .Include(tg => tg.TaxRates)
            .FirstOrDefaultAsync(tg => tg.Id == id);
    }

    public async Task<TaxGroup> AddAsync(TaxGroup taxGroup)
    {
        _context.TaxGroups.Add(taxGroup);

        await _context.SaveChangesAsync();

        return taxGroup;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<TaxGroup?> GetActiveByIdAsync(Guid id)
    {
        return await _context.TaxGroups
            .AsNoTracking()
            .Include(tg => tg.TaxRates.Where(tr => tr.IsActive))
            .FirstOrDefaultAsync(tg =>
                tg.Id == id &&
                tg.IsActive);
    }
}