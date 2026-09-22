using AddressManagement.Application;
using AddressManagement.Application.Dtos;
using AddressManagement.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Api.Controllers;

[ApiController]
[Route("api/addresses")]
[Produces("application/json")]
public class AddressController(
    ILogger<AddressController> logger,
    IAddressService addressService
    ) : ControllerBase
{
    [HttpGet(Name = "GetAll")]
    public async Task<ActionResult<IEnumerable<AddressListDto>>> GetAll(CancellationToken ct)
    {
        var addresses = await addressService.GetAll(ct);
        return Ok(addresses);
    }
    
    [HttpPost(Name = "Address")]
    public async Task<ActionResult<bool>> AddAddress([FromForm] AddressUpsertDto addressDto)
    {
        var addresses = await addressService.Add();
        return addresses;
    }
}