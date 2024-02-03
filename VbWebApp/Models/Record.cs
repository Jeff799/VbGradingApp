using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

  public virtual ICollection<Stat> Stats { get; set; }
}
