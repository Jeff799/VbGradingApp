using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using Models;

namespace Controllers;

public class PlayerController : Controller
{
  private readonly VbContext _context;
  private readonly ILogger<PlayerController> _logger;

  private int PlayerID => HttpContext.Session.GetInt32(nameof(Player.PlayerID)).Value;

  public PlayerController(VbContext context, ILogger<PlayerController> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<IActionResult> Index()
  {
    var players = await _context.Players.ToListAsync();
    return View(players);
  }

  [HttpGet]
  public IActionResult Add()
  {
    var model = new PlayerViewModel
    {
      PositionOptions = Enum.GetValues(typeof(Player.Position))
            .Cast<Player.Position>()
            .Select(p => new SelectListItem
            {
              Value = ((int)p).ToString(),
              Text = p.ToString()
            })
    };

    return View(model);
  }

  [HttpPost]
  public async Task<IActionResult> Add(PlayerViewModel model)
  {
    var existingPlayer = await _context.Players
      .AnyAsync(p => p.PlayerID == model.PlayerID);
    if (existingPlayer)
    {
      ModelState.AddModelError("PlayerID", "Player ID already exists.");
    }

    if (ModelState.IsValid)
    {
      Player player = new Player
      {
        PlayerID = model.PlayerID,
        FirstName = model.FirstName,
        LastName = model.LastName,
        Height = model.Height,
        Positions = (Player.Position)model.SelectedPositions.Aggregate(0, (current, position) => current | position)
      };

      _context.Players.Add(player);
      await _context.SaveChangesAsync();

      return RedirectToAction(nameof(Index)); // Redirect to index
    }

    // If the model state is not valid, repopulate the PositionOptions for the form
    model.PositionOptions = Enum.GetValues(typeof(Player.Position))
        .Cast<Player.Position>()
        .Select(p => new SelectListItem
        {
          Value = ((int)p).ToString(),
          Text = p.ToString()
        });
    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
    {
      _logger.LogWarning("Validation Error: {ErrorMessage}", error.ErrorMessage);
    }

    return View(model);
  }

}