using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;

namespace Models;
public class Player
{

  [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
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
    Middle = 4,
    Libero = 8,
    Opposite = 16

  }

  public Position Positions { get; set; }

  public virtual ICollection<Stat> Stats { get; set; }

}
