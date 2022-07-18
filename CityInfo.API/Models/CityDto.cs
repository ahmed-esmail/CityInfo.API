namespace CityInfo.API.Models;

public class CityDto
{
  public int Id { get; set; }
  public string Name { get; set; } = String.Empty;
  public string? Description { get; set; }
  public int NumberOfPointsOfInterest { get; set; }
}