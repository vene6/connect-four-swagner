using System.Text;

namespace ConnectFour {
  public class Grid : IGrid {
    public const int ROWS = 6;
    public const int COLS = 7;
    public const int WIN_COUNT = 4;

    private readonly Player[,] grid;

    public Player this[int r, int c] => grid[r, c];

    public virtual bool Full => grid.Cast<Player>().All(p => p != Player.None);

    public Grid() {
      grid = new Player[ROWS, COLS];
      Reset();
    }

    public Grid(Player[,] initialGrid) {
      if (IsInvalidGrid(initialGrid, out string msg)) throw new ArgumentException(msg, nameof(initialGrid));
      grid = initialGrid;
    }

    public override string ToString() {
      StringBuilder sb = new();
      for (int r = 0; r < ROWS; r++) {
        sb.Append('|');
        for (int c = 0; c < COLS; c++) {
          string s = grid[r, c] switch {
            Player.Red => " R ",
            Player.Yellow => " Y ",
            _ => " . "
          };
          sb.Append(s);
        }
        sb.Append('|').AppendLine();
      }
      return sb.ToString();
    }

    public virtual void Reset() {
      Array.Clear(grid);
    }

    public virtual bool Drop(Player player, int col) {
      if ((col < 0) || (col >= COLS)) throw new ArgumentException($"invalid column number (must be in interval [0, {COLS}[)", nameof(col));

      for (int r = ROWS - 1; r >=0 ; r--) {
        if (grid[r, col] == Player.None) {
          grid[r, col] = player;
          if (IsInvalidGrid(grid, out string msg)) {
            grid[r, col] = Player.None;
            throw new InvalidOperationException(msg);
          }
          return true;
        }
      }
      return false;
    }

    public virtual bool HasFourInARow(Player player) {
      for (int row = 0; row < ROWS; row++) {
        for (int col = 0; col < COLS; col++) {
          if (grid[row, col] == player) {
            // offsets:
            // (1,  0) ... vertical
            // (0,  1) ... horizontal
            // (1,  1) ... diagonal right
            // (1, -1) ... diagnoal left
            (int, int)[] offsets = new[] { (1,0), (0,1), (1,1), (1,-1) };
            foreach ((int rowOffset, int colOffset) in offsets) {
              int r = row;
              int c = col;
              int same = 0;
              while ((r >= 0) && (r < ROWS) &&
                     (c >= 0) && (c < COLS) &&
                     (grid[r, c] == player)) {
                same++;
                r += rowOffset;
                c += colOffset;
              }
              if (same >= WIN_COUNT) return true;
            }
          }
        }
      }
      return false;
    }

    private static bool IsInvalidGrid(Player[,] grid, out string message) {
      // invalid number of rows
      if (grid.GetLength(0) != ROWS) {
        message = $"invalid number of rows (must be {ROWS})";
        return true;
      }

      // invalid number of columns
      if (grid.GetLength(1) != COLS) {
        message = $"invalid number of columns (must be {COLS})";
        return true;
      }

      // invalid number of red and yellow discs
      // players switch after each move -> difference must not be larger than 1
      int reds = 0, yellows = 0;
      for (int r = 0; r < ROWS; r++) {
        for (int c = 0; c < COLS; c++) {
          if (grid[r, c] == Player.Yellow) yellows++;
          if (grid[r, c] == Player.Red) reds++;
        }
      }
      if (Math.Abs(reds - yellows) > 1) {
        message = "invalid number of reds and yellows";
        return true;
      }

      // invalid grid configuration
      // discs must be at the bottom
      for (int c = 0; c < COLS; c++) {
        bool foundNone = false;
        for (int r = ROWS - 1; r >= 0; r--) {
          if (grid[r, c] == Player.None) foundNone = true;
          if (foundNone && grid[r, c] != Player.None) {
            message = "discs of players must be at the bottom";
            return true;
          }
        }
      } 

      message = string.Empty;
      return false;
    }
  }
}
