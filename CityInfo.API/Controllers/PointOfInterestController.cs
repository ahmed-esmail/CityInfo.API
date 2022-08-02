using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/cities/{cityId:int}/pointsOfInterest")]
[ApiController]
public class PointOfInterestController : ControllerBase
{
  private readonly ILogger<PointOfInterestController> _logger;
  private readonly IMailService _mailService;
  private readonly CitiesDataStore _citiesDataStore;

  public PointOfInterestController(ILogger<PointOfInterestController> logger,
    IMailService mailService,
    CitiesDataStore _citiesDataStore)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
    this._citiesDataStore = _citiesDataStore;
  }
  
  [HttpGet]
  public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest([FromRoute] int cityId)
  {
    try
    {
      var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
      if (city != null) return Ok(city.PointsOfInterest);

      #region Logger
      _logger.LogInformation($"city with id = {cityId} wasn't found when accessing point of interest.");
      #endregion
      
      return NotFound();

    }
    catch (Exception ex)
    {
      _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
      return StatusCode(500, "A problem happened while handling your request");
    }
    
  }

  [HttpGet("{pointOfInterestId}")]
  public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId,[FromRoute] int pointOfInterestId)
  {
    var city = _citiesDataStore.Cities
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
    var cities = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == cityId);
    if (cities == null)
    {
      return NotFound();
    }
    var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(
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
    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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
    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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
    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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
    _mailService.Send("Point of interest deleted.",
      $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestId} was deleted.");
    return NoContent();
  }
}