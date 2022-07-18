using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId:int}/pointsOfInterest")]
[ApiController]
public class PointOfInterestController : ControllerBase
{
  [HttpGet]
  public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest([FromRoute] int cityId)
  {
    var city = CitiesDataStore.citiesData.Cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
    {
      return NotFound();
    }

    return Ok(city.PointsOfInterest);
  }

  [HttpGet("{pointOfInterestId}")]
  public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
  {
    var city = CitiesDataStore.citiesData.Cities
      .FirstOrDefault(c => c.Id == cityId);
    if (city == null)
    {
      return NotFound();
    }

    var pointOfInterest = city.PointsOfInterest
      .FirstOrDefault(p => p.Id == pointOfInterestId);
    if (pointOfInterest == null)
    {
      return NotFound();
    }

    return Ok(pointOfInterest);
  }
}