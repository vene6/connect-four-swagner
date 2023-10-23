namespace ConnectFour {
  public interface IGrid {
    Player this[int r, int c] { get; }
    bool Full { get; }

    void Reset();
    bool Drop(Player player, int col);
    bool HasFourInARow(Player player);
  }
}