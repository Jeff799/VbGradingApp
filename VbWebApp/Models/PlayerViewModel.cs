using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Models;
public class PlayerViewModel
{
  public int PlayerID { get; set; }

  [Required, StringLength(50)]
  public string FirstName { get; set; }

  public string LastName { get; set; }

  [StringLength(3, MinimumLength = 3, ErrorMessage = "Height must 3 letters long")]
  [RegularExpression("^[0-9]*$", ErrorMessage = "Height must be numerical")]
  public string Height { get; set; }

  public List<int> SelectedPositions { get; set; } = new List<int>();

  public IEnumerable<SelectListItem> PositionOptions { get; set; }
}
