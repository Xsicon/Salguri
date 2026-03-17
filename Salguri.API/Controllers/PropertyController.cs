using Microsoft.AspNetCore.Mvc;
using Salguri.Application.Interfaces;

namespace Salguri.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _propertyService.GetAllPropertiesAsync();
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _propertyService.GetPropertyByIdAsync(id);
        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

}
