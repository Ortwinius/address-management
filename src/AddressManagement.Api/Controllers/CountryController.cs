using AddressManagement.Application.Dtos;
using AddressManagement.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace AddressManagement.Api.Controllers;

[ApiController]
[Route("api/countries")]
[Produces("application/json")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CountryDto>>> GetAll(CancellationToken ct = default)
    {
        var countries = await countryService.GetAll(ct);
        return Ok(countries);
    }
}