using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models;
public class Record
{
  [Key]
  public int RecordID { get; set; }

  [Required]
  public char RecordType { get; set; }

  public decimal Point { get; set; }

  [StringLength(30)]
  public string? Comment { get; set; }

  public DateTime RecordTimeUtc { get; set; }

  public virtual Stat Stats { get; set; }
}
