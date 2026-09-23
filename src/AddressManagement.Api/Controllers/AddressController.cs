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
    public async Task<ActionResult<PagedResult<AddressListDto>>> GetAll(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var addresses = await addressService.GetAll(page, pageSize, ct);
        return Ok(addresses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AddressDetailDto>> GetById(int id, CancellationToken ct = default)
    {
        var address = await addressService.GetById(id, ct);
        return address is null ? NotFound() : Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDetailDto>> Add([FromBody] AddressCreateDto dto, CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var created = await addressService.Add(dto, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AddressDetailDto>> Update(int id, [FromBody] AddressCreateDto dto, CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var updated = await addressService.Update(id, dto, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct = default)
    {
        await addressService.Delete(id, ct);
        return Ok();
    }
}
