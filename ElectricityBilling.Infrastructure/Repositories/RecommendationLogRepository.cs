using ElectricityBilling.Application.Interfaces;
using ElectricityBilling.Domain.Entities;
using ElectricityBilling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ElectricityBilling.Infrastructure.Repositories
{
    public class RecommendationLogRepository
        : IRecommendationLogRepository
    {
        private readonly ApplicationDbContext _context;

        public RecommendationLogRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            RecommendationLog recommendationLog)
        {
            await _context.RecommendationLogs
                .AddAsync(recommendationLog);

            await _context.SaveChangesAsync();
        }

        public async Task<List<RecommendationLog>> GetAllWithDetailsAsync()
        {
            return await _context.RecommendationLogs
                .AsNoTracking()
                .Include(x => x.RecommendedPlan)
                .Include(x => x.TaxGroup)
                .ToListAsync();
        }
    }
}