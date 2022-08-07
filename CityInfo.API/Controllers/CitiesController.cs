using CityInfo.API.Filters;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;
[ApiController]
[Route("api/cities")]
[LogActionFilter]
public class CitiesController : ControllerBase
{
  private readonly CitiesDataStore _citiesDataStore;

  public CitiesController(CitiesDataStore citiesDataStore)
  {
    _citiesDataStore = citiesDataStore;
  }
  [HttpGet("{id}")]
  public IActionResult GetCity([FromRoute] int id)
  {
    var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);
   
    return city == null ? NotFound() : Ok(city);
  }

  // GET
  [HttpGet]
  public ActionResult<CitiesDataStore> Index()
  {
    var cityList = new CitiesDataStore();
    return Ok(cityList.Cities);
  }
}
