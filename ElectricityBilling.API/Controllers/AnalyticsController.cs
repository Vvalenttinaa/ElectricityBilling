using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityBilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(
            IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<AnalyticsSummaryDto>> GetSummary()
        {
            var result =
                await _analyticsService.GetSummaryAsync();

            return Ok(result);
        }
    }
}