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
    public async Task<ActionResult<AddressListItemDto>> GetAll()
    {
        logger.LogInformation("Fetching list of all addresses");
        try
        {
            var addresses = await addressService.GetAll();
            return Ok(addresses);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    
    // [HttpPost(Name = "Address")]
    // public async Task<ActionResult<bool>> AddAddress([FromForm] AddressUpsertDto addressDto)
    // {
    //     try
    //     {
    //         var addresses = await addressService.GetAll();
    //         return addresses;
    //     }
    //     catch (Exception e)
    //     {
    //         
    //     }
    // }
}