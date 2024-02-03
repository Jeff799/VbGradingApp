using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


public class Stat
{
  [Key, ForeignKey("Player")]
  public int PlayerID { get; set; }

  public decimal? PassingEfficiency { get; set; }

  public decimal? HittingEfficiency { get; set; }

  public virtual ICollection<Record> Records { get; set; }
}