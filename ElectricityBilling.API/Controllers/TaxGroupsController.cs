using ElectricityBilling.Application.DTOs.TaxGroupDto;
using ElectricityBilling.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityBilling.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class TaxGroupsController : ControllerBase
{
    private readonly ITaxGroupService _taxGroupService;

    public TaxGroupsController(
        ITaxGroupService taxGroupService)
    {
        _taxGroupService = taxGroupService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<TaxGroupDto>>> GetAll()
    {
        var taxGroups =
            await _taxGroupService.GetAllAsync();

        return Ok(taxGroups);
    }

    [HttpPost]
    public async Task<ActionResult<TaxGroupDto>> Create(
        CreateTaxGroupDto dto)
    {

        var taxGroup =
            await _taxGroupService.CreateAsync(dto);

        return Created(
            $"/api/taxgroups/{taxGroup.Id}",
            taxGroup);

    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaxGroupDto>> Update(
        Guid id,
        UpdateTaxGroupDto dto)
    {

        var taxGroup =
            await _taxGroupService.UpdateAsync(id, dto);

        if (taxGroup is null)
        {
            return NotFound(new
            {
                message = "Tax group not found."
            });
        }

        return Ok(taxGroup);

    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted =
            await _taxGroupService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Tax group not found."
            });
        }

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaxGroupDto>> GetById(Guid id)
    {
        var taxGroup =
            await _taxGroupService.GetByIdAsync(id);

        return Ok(taxGroup);
    }
}