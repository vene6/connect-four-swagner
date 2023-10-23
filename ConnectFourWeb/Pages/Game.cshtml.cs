using ConnectFour;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ConnectFourWeb.Pages {
  public class GameModel : PageModel {
    public static Dictionary<int, Game> Games { get; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Action { get; set; } = string.Empty;

    public Game Game { get; private set; } = new(Player.Yellow);
    public string this[int r, int c] => Game[r, c] switch {
        Player.Red => "R",
        Player.Yellow => "Y",
        _ => string.Empty
    };

    public void OnGet() {
      if (Id == 0) {
        Id = Games.Count + 1;
        Game = new Game(Player.Yellow);
        Games.Add(Id, Game);
      } else {
        Game = Games[Id];
      }

      if (!Game.GameOver) {
        switch (Action) {
          case "c1":
            Game.Drop(0);
            break;
          case "c2":
            Game.Drop(1);
            break;
          case "c3":
            Game.Drop(2);
            break;
          case "c4":
            Game.Drop(3);
            break;
          case "c5":
            Game.Drop(4);
            break;
          case "c6":
            Game.Drop(5);
            break;
          case "c7":
            Game.Drop(6);
            break;
        }
      }
    }
  }
}
