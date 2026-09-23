using AddressManagement.Application.Dtos;
using AddressManagement.Application.Services;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Api.Controllers;

[ApiController]
[Route("api/addresses")]
[Produces("application/json")]
public class AddressController(
    IAddressService addressService,
    IValidator<AddressCreateDto> validator
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AddressListDto>>> GetAll(CancellationToken ct)
    {
        var addresses = await addressService.GetAll(ct);
        return Ok(addresses);
    }

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IEnumerable<AddressListDto>>> GetById(int id, CancellationToken ct)
    {
        var addresses = await addressService.GetById(id, ct);
        return Ok(addresses);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDetailDto>> Add([FromBody] AddressCreateDto dto, CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var created = await addressService.Add(dto, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }
}
