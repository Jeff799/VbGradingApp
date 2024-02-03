using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

namespace Controllers;

public class PlayerController : Controller
{
  private readonly VbContext _context;

  private int PlayerID => HttpContext.Session.GetInt32(nameof(Player.PlayerID)).Value;

  public PlayerController(VbContext context)
  {
    _context = context;
  }

  public async Task<IActionResult> Index()
  {
    var players = await _context.Players.ToListAsync();
    return View(players);
  }
}