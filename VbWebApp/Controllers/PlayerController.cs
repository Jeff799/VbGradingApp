using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

  public async Task<IActionResult> Index(int pageNumber = 1)
  {
    const int pageSize = 4; // 4 players per page
    var players = await _context.Players.ToListAsync();
    var totalPlayers = players.Count;
    var paging = players.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    ViewBag.CurrentPage = pageNumber;
    ViewBag.TotalPages = (int)Math.Ceiling(totalPlayers / (double)pageSize);
    return View(paging);
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

    ModelState.Remove("PositionOptions"); // PositionOptions is only used to populate
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

  private List<int> ConvertPositionsToSelectedList(Player.Position positions)
  {
    var selectedPositions = new List<int>();
    foreach (var position in Enum.GetValues(typeof(Player.Position)).Cast<Player.Position>())
    {
      if (positions.HasFlag(position))
      {
        selectedPositions.Add((int)position);
      }
    }
    return selectedPositions;
  }


  [HttpGet]
  public async Task<IActionResult> Edit(int id) // Ensure you're passing the PlayerID as a parameter
  {
    var player = await _context.Players.FindAsync(id);
    if (player == null) return NotFound();

    var model = new PlayerViewModel
    {
      PlayerID = player.PlayerID,
      FirstName = player.FirstName,
      LastName = player.LastName,
      Height = player.Height,
      SelectedPositions = ConvertPositionsToSelectedList(player.Positions),
      PositionOptions = Enum.GetValues(typeof(Player.Position))
                              .Cast<Player.Position>()
                              .Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() })
    };

    return View(model);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(PlayerViewModel model)
  {
    ModelState.Remove("PositionOptions");
    if (!ModelState.IsValid)
    {
      // Repopulate PositionOptions if returning to view due to validation failure
      model.PositionOptions = Enum.GetValues(typeof(Player.Position))
          .Cast<Player.Position>()
          .Select(p => new SelectListItem { Value = ((int)p).ToString(), Text = p.ToString() });
      foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
      {
        _logger.LogWarning("Validation Error: {ErrorMessage}", error.ErrorMessage);
      }
      return View(model);
    }

    var player = await _context.Players.FindAsync(model.PlayerID);
    if (player == null) return NotFound();

    player.FirstName = model.FirstName;
    player.LastName = model.LastName;
    player.Height = model.Height;
    player.Positions = (Player.Position)model.SelectedPositions.Aggregate(0, (current, position) => current | position);

    _context.Update(player);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
  }

  public async Task<IActionResult> Delete(int id)
  {
    var player = await _context.Players.FindAsync(id);
    if (player == null)
    {
      return NotFound();
    }
    ViewBag.FullName = player.FullName;
    return View("ConfirmDelete", player);
  }

  [HttpPost]
  public async Task<IActionResult> ConfirmDelete(int id)
  {
    var player = await _context.Players.FindAsync(id);
    if (player == null)
    {
      return NotFound();
    }

    // else
    _context.Players.Remove(player);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));

  }


}