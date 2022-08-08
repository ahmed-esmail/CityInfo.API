using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
  Task<IEnumerable<City>> GetCitiesAsync();
  Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
  Task<bool> IsCityExist(int cityId);
  Task<IEnumerable<PointOfInterest?>> GetPointsOfInterestForCityAsync(int cityId);
  Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
  Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
  void DeletePointOfInterest(PointOfInterest pointOfInterestEntity);
  Task<bool> SaveChangesAsync();
  
}