namespace CityInfo.API.Models
{
  public class CitiesDataStore
  {
    public List<CityDto> Cities { get; set; }
    public static CitiesDataStore citiesData { get; set; } = new CitiesDataStore();
    public CitiesDataStore()
    {
      Cities = new List<CityDto>()
      {
        new CityDto{ Id= 1, Name = "New York", Description="this one with big park"},
        new CityDto{ Id= 3, Name = "New York", Description="this one with big park"},
        new CityDto{ Id= 2, Name = "Paris", Description="this one with large park"}

      };
    }
  }
}
