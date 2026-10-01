using ElectricityBilling.Domain.Entities;

namespace ElectricityBilling.Application.Interfaces
{
    public interface IRecommendationLogRepository
    {
        Task AddAsync(RecommendationLog recommendationLog);
        Task<List<RecommendationLog>> GetAllWithDetailsAsync();
    }
}
