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
    IValidator<AddressCreateDto> validator,
    IValidator<AddressQueryDto> queryValidator
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<AddressListDto>>> GetAll(
        [FromQuery] AddressQueryDto queryDto, CancellationToken ct = default)
    {
        var validation = await queryValidator.ValidateAsync(queryDto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var addresses = await addressService.GetAll(queryDto, ct);
        return Ok(addresses);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AddressDetailDto>> GetById(int id, CancellationToken ct = default)
    {
        var address = await addressService.GetById(id, ct);
        return address is null ? NotFound() : Ok(address);
    }

    [HttpPost]
    public async Task<ActionResult<AddressDetailDto>> Add([FromBody] AddressCreateDto addressDto, CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(addressDto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var created = await addressService.Add(addressDto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AddressDetailDto>> Update(int id, [FromBody] AddressCreateDto addressDto, CancellationToken ct = default)
    {
        var validation = await validator.ValidateAsync(addressDto, ct);
        if (!validation.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(validation.ToDictionary()));
        }

        var updated = await addressService.Update(id, addressDto, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct = default)
    {
        var deleted = await addressService.Delete(id, ct);
        return deleted ? NoContent() : NotFound();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteMany([FromQuery] int[] ids, CancellationToken ct = default)
    {
        await addressService.DeleteMany(ids, ct);
        return NoContent();
    }
}