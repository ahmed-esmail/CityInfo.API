using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
  // GET
  [HttpGet]
  public ActionResult<CitiesDataStore> Index()
  {
    var cityList = new CitiesDataStore();
    return Ok(cityList);
  }

  [HttpGet("{id}")]
  public IActionResult GetCity([FromRoute] int id)
  {
    var city = CitiesDataStore.citiesData.Cities.FirstOrDefault(city => city.Id == id);

    return city == null ? NotFound() : Ok(city);
  }
}