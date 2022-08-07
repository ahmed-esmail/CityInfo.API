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

  // [HttpGet("{id}")]
  // public IActionResult GetCity([FromRoute] int id)
  // {
  //   var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);
  //  
  //   return city == null ? NotFound() : Ok(city);
  // }

  // GET
  [HttpGet]
  public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
  {
    var cities = await _cityInfoRepository.GetCitiesAsync();
    
    return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities));
  }
}
