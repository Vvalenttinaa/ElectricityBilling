using ElectricityBilling.Application.DTOs;

namespace ElectricityBilling.Application.Interfaces
{
    public interface IRecommendationService
    {
        Task<RecommendationResponseDto> GetRecommendationAsync(
            RecommendationRequestDto request);
    }
}
