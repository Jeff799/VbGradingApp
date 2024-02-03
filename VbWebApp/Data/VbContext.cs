using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class VbContext : DbContext
{
  public VbContext(DbContextOptions<VbContext> options) : base(options)
  { }

  public DbSet<Player> Players { get; set; }
  public DbSet<Login> Logins { get; set; }
  public DbSet<Record> Records { get; set; }
  public DbSet<Stat> Stats { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.Entity<Login>().ToTable(b =>
    {
      b.HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8");
      b.HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 94");
    });
  }
}