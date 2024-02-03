using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;


public class Player
{

  [Key]
  public int PlayerID { get; set; }

  [Required, StringLength(50)]
  public string FirstName { get; set; }

  public string LastName { get; set; }

  public string FullName => $"{FirstName} {LastName}";

  [StringLength(3, MinimumLength = 3, ErrorMessage = "Height must 3 letters long")]
  [RegularExpression("^[0-9]*$", ErrorMessage = "Height must be numerical")]
  public string Height { get; set; }

  [Flags]
  public enum Position
  {
    Outside = 1,
    Setter = 2,
    Middle = 3,
    Libero = 4,
    Opposite = 5

  }

  public Position Postiions { get; set; }

  public virtual ICollection<Stat> Stats { get; set; }

  public virtual Login Login { get; set; }
}