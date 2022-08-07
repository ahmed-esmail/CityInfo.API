using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;
[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
  private readonly ICityInfoRepository _cityInfoRepository;
  private readonly IMapper _mapper;

  public CitiesController(
    ICityInfoRepository cityInfoRepository,
    IMapper mapper
    )
  {
    _cityInfoRepository = cityInfoRepository;
    _mapper = mapper;
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetCity(
    [FromRoute] int id, bool includePointOfInterest = false
    )
  {
    var city = await _cityInfoRepository.GetCityAsync(id, includePointOfInterest);

    if (city == null)
    {
      return NotFound();
    }

    return includePointOfInterest ? Ok(_mapper.Map<CityDto>(city)) : Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
  }

  // GET
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
  {
    var cities = await _cityInfoRepository.GetCitiesAsync();
    
    return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
  }
}
