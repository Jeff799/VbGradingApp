using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


public class Login
{
  [Key, ForeignKey("Player")]
  public int PlayerID { get; set; }

  [Required, StringLength(8)]
  public string LoginID { get; set; }

  [Required, StringLength(94)]
  public string PasswordHash { get; set; }

  public virtual Player Player { get; set; }
}