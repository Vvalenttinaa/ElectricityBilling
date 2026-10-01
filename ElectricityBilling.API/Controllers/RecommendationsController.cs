using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityBilling.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationsController(
        IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpPost]
    public async Task<ActionResult<RecommendationResponseDto>> GetRecommendation(
        RecommendationRequestDto request)
    {
        var result =
            await _recommendationService.GetRecommendationAsync(request);

        return Ok(result);
    }
}