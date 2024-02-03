using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Models;

public class Login
{

  [Required, StringLength(8)]
  public string LoginID { get; set; }

  [Required, StringLength(94)]
  public string PasswordHash { get; set; }

}