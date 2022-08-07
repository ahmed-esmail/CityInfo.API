using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities;

public class PointOfInterest
{
  public PointOfInterest(string name)
  {
    Name = name;
  }
  
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  public string Name { get; set; }
  [MaxLength(255)]
  public string Description { get; set; }
  [ForeignKey("CityId")]
  public City City { get; set; }
  public int CityId { get; set; }
  
  
}