using Models;

namespace Data;

public static class SeedData
{
  public static void Initialize(IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetRequiredService<VbContext>();

    if (context.Players.Any())
      return; // already seeded

    context.Players.AddRange(
      new Player { PlayerID = 1, FirstName = "Jeff", LastName = "Lim", Height = "169", Positions = Player.Position.Outside | Player.Position.Setter },
      new Player { PlayerID = 2, FirstName = "Mingus", LastName = "GY", Height = "185", Positions = Player.Position.Outside | Player.Position.Setter | Player.Position.Middle },
      new Player { PlayerID = 3, FirstName = "Ben", LastName = "Wu", Height = "168", Positions = Player.Position.Outside },
      new Player { PlayerID = 4, FirstName = "Jono", LastName = "Liu", Height = "171", Positions = Player.Position.Outside },
      new Player { PlayerID = 5, FirstName = "Jeff", LastName = "Huang", Height = "179", Positions = Player.Position.Outside | Player.Position.Setter | Player.Position.Middle }
    );

    context.Logins.AddRange(
      new Login
      {

        LoginID = "12345678",
        PasswordHash =
                    "Rfc2898DeriveBytes$50000$MrW2CQoJvjPMlynGLkGFrg==$x8iV0TiDbEXndl0Fg8V3Rw91j5f5nztWK1zu7eQa0EE="
      }
    );
    context.SaveChanges();
  }
}
