using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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
  public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId,[FromRoute] int pointOfInterestId)
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

  [HttpPost]
  public ActionResult<PointOfInterestDto> CreatePointOfInterest
    ([FromBody] PointOfInterestForCreationDto pointOfInterest, [FromRoute] int cityId)
  {
    var cities = CitiesDataStore.citiesData.Cities.FirstOrDefault(city => city.Id == cityId);
    if (cities == null)
    {
      return NotFound();
    }
    var maxPointOfInterestId = CitiesDataStore.citiesData.Cities.SelectMany(
      c => c.PointsOfInterest).Max(p => p.Id);

    var finalPointOfInterest = new PointOfInterestDto()
    {
      Id = ++maxPointOfInterestId,
      Name = pointOfInterest.Name,
      Description = pointOfInterest.Description
    };
    
    cities.PointsOfInterest.Add(finalPointOfInterest);
    
    return CreatedAtRoute(nameof(GetPointOfInterest), new {cityId, pointOfInterestId = finalPointOfInterest.Id}, finalPointOfInterest);
  }

  [HttpPut("{pointofinterestId}")]
  public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId,
    PointOfInterestForUpdateDto pointOfInterest)
  {
    var city = CitiesDataStore.citiesData.Cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
    {
      return NotFound();
    }
    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
    if (pointOfInterestFromStore == null)
    {
      return NotFound();
    }
    pointOfInterestFromStore.Name = pointOfInterest.Name;
    pointOfInterestFromStore.Description = pointOfInterest.Description;
  
    return NoContent();
  }
  
  [HttpPatch("{pointOfInterestId}")]
  public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
    JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
  {
    var city = CitiesDataStore.citiesData.Cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
    {
      return NotFound();
    }
    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
    if (pointOfInterestFromStore == null)
    {
      return NotFound();
    }
    var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
    {
      Name = pointOfInterestFromStore.Name,
      Description = pointOfInterestFromStore.Description
    };
    patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    if (!TryValidateModel(pointOfInterestToPatch))
    {
      return BadRequest(ModelState);
    }
    pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
    pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
    return NoContent();
  }

  [HttpDelete("{pointOfInterestId}")]
  public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
  {
    var city = CitiesDataStore.citiesData.Cities.FirstOrDefault(c => c.Id == cityId);
    if (city == null)
    {
      return NotFound();
    }
    var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);

    if (pointOfInterestFromStore == null)
    {
      return NotFound();
    }
    city.PointsOfInterest.Remove(pointOfInterestFromStore);
    return NoContent();
  }
}