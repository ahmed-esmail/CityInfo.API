namespace CityInfo.API.Models
{
  public class CitiesDataStore
  {
    public List<CityDto> Cities { get; set; }

    public CitiesDataStore()
    {
      // init dummy data
      Cities = new List<CityDto>()
      {
        
      };
    }
  }
}