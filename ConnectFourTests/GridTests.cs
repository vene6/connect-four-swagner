using FluentAssertions;
using ConnectFour;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConnectFourTests {
  public class GridTests {
    private const Player N = Player.None;
    private const Player Y = Player.Yellow;
    private const Player R = Player.Red;

    private static readonly Player[,] EMPTY_GRID = new[,] {
      { N, N, N, N, N, N, N },
      { N, N, N, N, N, N, N },
      { N, N, N, N, N, N, N },
      { N, N, N, N, N, N, N },
      { N, N, N, N, N, N, N },
      { N, N, N, N, N, N, N }
    };

    private static void AssertGrid(Player[,] expected, Grid grid) {
      for (int r = 0; r < Grid.ROWS; r++) {
        for (int c = 0; c < Grid.COLS; c++) {
          Assert.Equal(expected[r, c], grid[r, c]);
        }
      }
    }

    [Fact]
    public void Ctor_ReturnsNewInstance() {
      Grid sut = new();

      for (int r = 0; r < Grid.ROWS; r++) {
        for (int c = 0; c < Grid.COLS; c++) {
          Assert.Equal(Player.None, sut[r, c]);
        }
      }
    }

    [Theory]
    [MemberData(nameof(Ctor_WithValidInitialGrid_ReturnsInitializedInstance_Data))]
    public void Ctor_WithValidInitialGrid_ReturnsInitializedInstance(Player[,] initialGrid) {
      Grid sut = new(initialGrid);
      AssertGrid(initialGrid, sut);
    }
    #region Ctor_WithValidInitialGrid_ReturnsInitializedInstance_Data
    public static IEnumerable<object[]> Ctor_WithValidInitialGrid_ReturnsInitializedInstance_Data() {
      yield return new object[] {
        EMPTY_GRID
      };
      yield return new object[] {
        new Player[,] {
          { R, Y, R, Y, R, Y, R },
          { Y, R, Y, R, Y, R, Y },
          { R, Y, R, Y, R, Y, R },
          { Y, R, Y, R, Y, R, Y },
          { R, Y, R, Y, R, Y, R },
          { Y, R, Y, R, Y, R, Y }
        }
      };
    }
    #endregion

    [Theory]
    [MemberData(nameof(Ctor_WithInvalidInitialGrid_ThrowsException_Data))]
    public void Ctor_WithInvalidInitialGrid_ThrowsException(Player[,] invalidGrid) {
      Assert.Throws<ArgumentException>(() => new Grid(invalidGrid));
    }
    #region Ctor_WithInvalidInitialGrid_ThrowsException_Data
    public static IEnumerable<object[]> Ctor_WithInvalidInitialGrid_ThrowsException_Data() {
      yield return new object[] {
        new Player[0, 0]
      };
      yield return new object[] {
        new Player[,] {
          { N, N },
          { N, N }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N },
          { N },
          { N },
          { N },
          { N },
          { N }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N }
        }
      };
      yield return new object[] {  // too many red, not enough yellow
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { R, R, R, R, R, R, Y }
        }
      };
      yield return new object[] {  // discs are not at the bottom
        new Player[,] {
          { R, Y, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N }
        }
      };
    }
    #endregion

    [Theory]
    [MemberData(nameof(Reset_ClearsGrid_Data))]
    public void Reset_ClearsGrid(Player[,] initialGrid) {
      Grid sut = new(initialGrid);
      sut.Reset();
      AssertGrid(EMPTY_GRID, sut);
    }
    #region Reset_ClearsGrid_Data
    public static IEnumerable<object[]> Reset_ClearsGrid_Data() {
      yield return new object[] {
        EMPTY_GRID
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, R, Y, N, N, N }
        }
      };
    }
    #endregion

    [Theory]
    [MemberData(nameof(Drop_WithValidColumn_ReturnsTrue_Data))]
    public void Drop_WithValidColumn_ReturnsTrue(Player[,] initialGrid, int col, Player[,] expectedGrid) {
      Grid sut = new(initialGrid);
      bool result = sut.Drop(Player.Yellow, col);
      Assert.True(result);
      AssertGrid(expectedGrid, sut);
    }
    #region Drop_WithValidColumn_ReturnsTrue_Data
    public static IEnumerable<object[]> Drop_WithValidColumn_ReturnsTrue_Data() {
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N }
        },
        0,  // column
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { Y, N, N, N, N, N, N }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R }
        },
        6,  // column
        new Player[,] {
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R }
        }
      };
    }
    #endregion

    [Theory]
    [MemberData(nameof(Drop_WhenColumnIsFull_ReturnsFalse_Data))]
    public void Drop_WhenColumnIsFull_ReturnsFalse(Player[,] initialGrid, int col) {
      Grid sut = new(initialGrid);
      Assert.False(sut.Drop(Player.Yellow, col));
    }
    #region Drop_WhenColumnIsFull_ReturnsFalse_Data
    public static IEnumerable<object[]> Drop_WhenColumnIsFull_ReturnsFalse_Data() {
      yield return new object[] {
        new Player[,] {
          { R, N, N, N, N, N, N },
          { Y, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { Y, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { Y, N, N, N, N, N, N }
        },
        0  // column
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, N, Y }
        },
        6  // column
      };
    }
    #endregion

    [Theory]
    [InlineData(-1)]
    [InlineData(7)]
    public void Drop_WithInvalidColumn_ThrowsException(int invalidCol) {
      Grid sut = new();
      Assert.Throws<ArgumentException>(() => sut.Drop(Player.Red, invalidCol));
    }

    [Fact]
    public void Drop_WithInvalidSequence_ThrowsException() {
      Grid sut = new();

      Assert.Throws<InvalidOperationException>(() => {
        sut.Drop(R, 0);
        sut.Drop(Y, 0);
        sut.Drop(R, 0);
        sut.Drop(R, 0);
        sut.Drop(R, 0);
      });
    }

    [Fact]
    public void Full_WhenGridIsFull_ReturnsTrue() {
      Player[,] initialGrid = new[,] {
        { R, Y, R, Y, R, Y, R },
        { Y, R, Y, R, Y, R, Y },
        { R, Y, R, Y, R, Y, R },
        { Y, R, Y, R, Y, R, Y },
        { R, Y, R, Y, R, Y, R },
        { Y, R, Y, R, Y, R, Y }
      };
      Grid sut = new(initialGrid);
      Assert.True(sut.Full);
    }

    [Fact]
    public void Full_WhenGridIsNotFull_ReturnsFalse() {
      Grid sut = new(EMPTY_GRID);
      Assert.False(sut.Full);
    }


    [Theory]
    [MemberData(nameof(HasFourInARow_WhenRedPlayerWins_ReturnsTrue_Data))]
    public void HasFourInARow_WhenRedPlayerWins_ReturnsTrue(Player[,] initialGrid) {
      Grid sut = new(initialGrid);
      Assert.True(sut.HasFourInARow(Player.Red));
    }
    #region HasFourInARow_WhenRedPlayerWins_ReturnsTrue_Data
    public static IEnumerable<object[]> HasFourInARow_WhenRedPlayerWins_ReturnsTrue_Data() {
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, Y, Y },
          { R, R, R, R, N, Y, Y }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { R, N, N, N, N, Y, Y },
          { R, N, N, N, N, Y, Y }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { Y, R, N, N, N, N, N },
          { Y, Y, R, N, N, N, N },
          { Y, Y, Y, R, N, R, R }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, R },
          { N, N, N, N, N, R, Y },
          { N, N, N, N, R, Y, Y },
          { R, R, N, R, Y, Y, Y }
        }
      };
    }
    #endregion

    [Theory]
    [MemberData(nameof(HasFourInARow_WhenRedPlayerDoesNotWin_ReturnsFalse_Data))]
    public void HasFourInARow_WhenRedPlayerDoesNotWin_ReturnsFalse(Player[,] initialGrid) {
      Grid sut = new(initialGrid);
      Assert.False(sut.HasFourInARow(Player.Red));
    }
    #region HasFourInARow_WhenRedPlayerDoesNotWin_ReturnsFalse_Data
    public static IEnumerable<object[]> HasFourInARow_WhenRedPlayerDoesNotWin_ReturnsFalse_Data() {
      yield return new object[] {
        EMPTY_GRID
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, Y, Y },
          { R, R, R, N, N, Y, Y }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { R, N, N, N, N, N, N },
          { R, N, N, N, N, Y, Y },
          { R, N, N, N, N, Y, Y }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { Y, R, N, N, N, N, N },
          { Y, Y, R, N, N, N, N },
          { Y, Y, Y, R, N, R, R }
        }
      };
      yield return new object[] {
        new Player[,] {
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, N, N },
          { N, N, N, N, N, R, Y },
          { N, N, N, N, R, Y, Y },
          { R, R, N, R, Y, Y, Y }
        }
      };
    }
    #endregion

    [Fact]
    public void ToString_ReturnsNonEmptyString() {
      Grid sut = new();
      string result = sut.ToString();
      result.Should().NotBeNullOrEmpty();
    }
  }
}
