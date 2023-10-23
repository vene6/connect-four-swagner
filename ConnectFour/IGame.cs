namespace ConnectFour {
  public interface IGame {
    Player this[int r, int c] { get; }
    Player PlayerOnTurn { get; }
    bool GameOver { get; }
    Player Winner { get; }

    void Reset(Player playerOnTurn);
    void Drop(int col);
  }
}
