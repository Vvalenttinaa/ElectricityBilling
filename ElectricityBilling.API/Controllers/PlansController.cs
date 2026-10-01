using ElectricityBilling.Application.DTOs;
using ElectricityBilling.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityBilling.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class PlansController : ControllerBase
{
    private readonly IPlanService _planService;

    public PlansController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlanDto>>> GetAll()
    {
        var plans = await _planService.GetAllAsync();

        return Ok(plans);
    }


    [HttpPost]
    public async Task<ActionResult<PlanDto>> Create(
        CreatePlanDto dto)
    {

        var plan = await _planService.CreateAsync(dto);

        return Created(
            $"/api/plans/{plan.Id}",
            plan);

    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PlanDto>> Update(
    Guid id,
    UpdatePlanDto dto)
    {

        Console.WriteLine($"Updating plan with ID: {id}");
        Console.WriteLine($"Update DTO: Name={dto.Name}, Description={dto.Description}, Discount={dto.Discount}, IsActive={dto.IsActive}, PricingTiersCount={dto.PricingTiers?.Count}");
        var plan = await _planService.UpdateAsync(id, dto);

        if (plan is null)
        {
            return NotFound(new
            {
                message = "Plan not found."
            });
        }

        return Ok(plan);


    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _planService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Plan not found."
            });
        }

        return NoContent();
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PlanDto>> GetById(Guid id)
    {
        var plan = await _planService.GetByIdAsync(id);

        return Ok(plan);
    }
}