using AutoMapper;
using CityInfo.API.Entities;
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
  private readonly ICityInfoRepository _cityInfoRepository;
  private readonly IMapper _mapper;

  public PointOfInterestController(ILogger<PointOfInterestController> logger,
    IMailService mailService,
    ICityInfoRepository cityInfoRepository,
    IMapper mapper)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
    _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
    _mapper = mapper;
  }
  
  [HttpGet]
  public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest([FromRoute] int cityId)
  {
    try
    {
      if (! await _cityInfoRepository.IsCityExist(cityId))
      {
        #region Logger
        _logger.LogInformation($"city with id = {cityId} wasn't found when accessing point of interest.");
        #endregion

        return NotFound();
      }

      var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

      return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

    }
    catch (Exception ex)
    {
      #region Logger
      _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
      #endregion
      return StatusCode(500, "A problem happened while handling your request");
    }
    
  }

  [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
  public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId,int pointOfInterestId)
  {
   
    if (! await _cityInfoRepository.IsCityExist(cityId))
    {
      #region Logger
      _logger.LogInformation($"city with id = {cityId} wasn't found when accessing point of interest.");
      #endregion
      return NotFound();
    }

    var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
    if (pointOfInterest == null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
  }

  [HttpPost]
  public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest
    ([FromBody] PointOfInterestForCreationDto pointOfInterest, [FromRoute] int cityId)
  {
    if (!await _cityInfoRepository.IsCityExist(cityId))
    {
      return NotFound();
    }

    var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

    await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
    await _cityInfoRepository.SaveChangesAsync();

    var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

    return CreatedAtRoute(nameof(GetPointOfInterest),
      new {cityId, pointOfInterestId = createdPointOfInterestToReturn.Id},
      createdPointOfInterestToReturn);
  }
  
  [HttpPut("{pointofinterestId}")]
  public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
    PointOfInterestForUpdateDto pointOfInterest)
  {
    if (!await _cityInfoRepository.IsCityExist(cityId))
    {
      return NotFound();
    }
    var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
    if (pointOfInterestEntity == null)
    {
      return NotFound();
    }

    _mapper.Map(pointOfInterest, pointOfInterestEntity);
    await _cityInfoRepository.SaveChangesAsync();
    
    return NoContent();
  }
  
  [HttpPatch("{pointOfInterestId}")]
  public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId,
    JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
  {
    
    if (! await _cityInfoRepository.IsCityExist(cityId))
    {
      return NotFound();
    }

    var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
    if (pointOfInterestEntity == null)
    {
      return NotFound();
    }

    var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
    patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }
  
    if (!TryValidateModel(pointOfInterestToPatch))
    {
      return BadRequest(ModelState);
    }

    _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
    await _cityInfoRepository.SaveChangesAsync();
    
    return NoContent();
  }
  
  [HttpDelete("{pointOfInterestId}")]
  public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
  {
    if (! await _cityInfoRepository.IsCityExist(cityId))
      return NotFound();

    var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

    if (pointOfInterestEntity == null)
      return NotFound();
    
    _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
    await _cityInfoRepository.SaveChangesAsync();

    _mailService.Send("Point of interest deleted.",
      $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");
    return NoContent();
  }
}