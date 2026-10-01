using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;
using ElectricityBilling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectricityBilling.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly ApplicationDbContext _context;

    public PlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Plan>> GetActivePlansWithPricingTiersAsync()
    {
        return await _context.Plans
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Include(p => p.PricingTiers)
            .ToListAsync();
    }

    public async Task<List<Plan>> GetAllAsync()
    {
        return await _context.Plans
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Include(p => p.PricingTiers)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Plan> AddAsync(Plan plan)
    {
        _context.Plans.Add(plan);

        await _context.SaveChangesAsync();

        return plan;
    }

    public async Task<Plan?> GetByIdForUpdateAsync(Guid id)
    {
        return await _context.Plans
            .Include(p => p.PricingTiers)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void RemovePricingTiers(IEnumerable<PricingTier> tiers)
    {
        _context.PricingTiers.RemoveRange(tiers);
    }

    public async Task<Plan?> GetByIdAsync(Guid id)
    {
        return await _context.Plans
            .AsNoTracking()
            .Include(p => p.PricingTiers)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }
}