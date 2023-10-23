using System.Text;

namespace ConnectFour {
  public class Game : IGame {
    private readonly IGrid grid = new Grid();

    public Player this[int r, int c] => grid[r, c];
    public Player PlayerOnTurn { get; private set; }
    public bool GameOver { get; private set; }
    public Player Winner { get; private set; }


    public Game(Player playerOnTurn) {
      Reset(playerOnTurn);
    }
    public Game(IGrid initialGrid, Player playerOnTurn) : this(playerOnTurn) {
      grid = initialGrid;
    }


    public override string ToString() {
      StringBuilder sb = new();
      sb.AppendLine($"Player: {PlayerOnTurn.ToString().ToUpper()}");
      sb.AppendLine();
      sb.AppendLine(grid.ToString());
      return sb.ToString();
    }

    public void Reset(Player playerOnTurn) {
      if (playerOnTurn == Player.None) throw new ArgumentException("player on turn must not be None", nameof(playerOnTurn));
      grid.Reset();
      PlayerOnTurn = playerOnTurn;
      GameOver = false;
      Winner = Player.None;
    }

    public void Drop(int col) {
      if (grid.Drop(PlayerOnTurn, col)) {
        if (grid.HasFourInARow(PlayerOnTurn)) {
          Winner = PlayerOnTurn;
          GameOver = true;
        } else if (grid.Full) {
          GameOver = true;
        } else {
          PlayerOnTurn = PlayerOnTurn == Player.Yellow ? Player.Red : Player.Yellow;
        }
      }
    }
  }
}
