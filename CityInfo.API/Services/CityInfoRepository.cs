using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository: ICityInfoRepository
{
  private readonly CityInfoContext _context;

  public CityInfoRepository(CityInfoContext context)
  {
    _context = context ?? throw new ArgumentNullException(nameof(context));
  }
  
  public async Task<IEnumerable<City>> GetCitiesAsync()
  {
    return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
  }

  public async Task<City?> GetCityAsync(int cityId, bool includePointOfInterest)
  {
    if (includePointOfInterest)
    {
      await _context.Cities.Include(c => c.PointsOfInterest)
        .Where(c => c.Id == cityId)
        .FirstOrDefaultAsync();
    }
    return await _context.Cities.FindAsync(cityId);
  }

  public async Task<bool> IsCityExist(int cityId)
  {
    return  await _context.Cities.AnyAsync(c => c.Id == cityId);
  }

  public async Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId)
  {
    return await _context.PointOfInterests
      .Where(p => p != null && p.CityId == cityId)
      .ToListAsync();
  }

  public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(
    int cityId,
    int pointOfInterestId)
  {
    return await _context.PointOfInterests
      .Where(p => p != null && p.CityId == cityId && p.Id == pointOfInterestId)
      .FirstOrDefaultAsync();
  }

  public async Task AddPointOfInterestForCity(int cityId,
    PointOfInterest pointOfInterest)
  {
    var city = await GetCityAsync(cityId, false);
    if (city != null)
    {
      city.PointsOfInterest.Add(pointOfInterest);
    }

  }

  public async Task<bool> SaveChangesAsync()
  {
   return (await _context.SaveChangesAsync() >= 0);
  }
}