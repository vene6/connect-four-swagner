using ConnectFour;

Console.WriteLine("Connect Four:");
Console.WriteLine("=============");
Console.WriteLine();

Game game = new(Player.Yellow);
Console.WriteLine(game);

while (!game.GameOver) {
  Console.Write("Command [1 .. 7, (r)estart, (q)uit, (h)elp] > ");

  string input = Console.ReadLine() ?? string.Empty;
  switch (input) {
    case "1": game.Drop(0); break;
    case "2": game.Drop(1); break;
    case "3": game.Drop(2); break;
    case "4": game.Drop(3); break;
    case "5": game.Drop(4); break;
    case "6": game.Drop(5); break;
    case "7": game.Drop(6); break;
    case "r": game.Reset(Player.Yellow); break;
    case "q": Console.WriteLine("Ok, bye."); return;
    case "h": printHelp(); break;
    default: Console.WriteLine("Unknown command"); break;
  }
  Console.WriteLine(game);
}

Console.WriteLine($"GAME OVER - Winner: {game.Winner.ToString().ToUpper()}");

static void printHelp() {
  Console.WriteLine();
  Console.WriteLine("Available commands:");
  Console.WriteLine("-------------------");
  Console.WriteLine("1 .. 7 --> drop disc in column");
  Console.WriteLine("r      --> restart game");
  Console.WriteLine("q      --> quit game");
  Console.WriteLine("h      --> show help");
}
