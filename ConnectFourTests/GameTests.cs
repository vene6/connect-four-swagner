using FluentAssertions;
using ConnectFour;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConnectFourTests {
  public class GameTests {
    [Theory]
    [InlineData(Player.Yellow)]
    [InlineData(Player.Red)]
    public void Ctor_WhenPlayerOnTurnIsValid_ReturnsNewInstance(Player playerOnTurn) {
      Game sut = new(playerOnTurn);
      Assert.Equal(playerOnTurn, sut.PlayerOnTurn);
      Assert.False(sut.GameOver);
      Assert.Equal(Player.None, sut.Winner);
    }

    [Fact]
    public void Ctor_WhenPlayerOnTurnIsInvalid_ThrowsArgumentException() {
      Assert.Throws<ArgumentException>(() => {
        Game sut = new(Player.None);
      });
    }

    [Fact]
    public void ToString_ReturnsNonEmptyString() {
      Game sut = new(Player.Yellow);
      string result = sut.ToString();
      result.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(Player.Yellow)]
    [InlineData(Player.Red)]
    public void Reset_WhenPlayerOnTurnIsValid(Player playerOnTurn) {
      var grid = new Mock<Grid>();
      Game sut = new(grid.Object, Player.Yellow);

      sut.Reset(playerOnTurn);

      Assert.Equal(playerOnTurn, sut.PlayerOnTurn);
      Assert.False(sut.GameOver);
      Assert.Equal(Player.None, sut.Winner);
      grid.Verify(x => x.Reset());
    }

    [Fact]
    public void Reset_WhenPlayerOnTurnIsInvalid_ThrowsArgumentException() {
      Game sut = new(Player.Yellow);
      Assert.Throws<ArgumentException>(() => {
        sut.Reset(Player.None);
      });
    }

    [Fact]
    public void Drop_WhenGameIsWon() {
      Player player = Player.Yellow;
      var grid = new Mock<Grid>();
      grid.Setup(x => x.Drop(player, 0)).Returns(true);
      grid.Setup(x => x.HasFourInARow(player)).Returns(true);
      Game sut = new(grid.Object, player);

      sut.Drop(0);

      Assert.Equal(player, sut.Winner);
      Assert.True(sut.GameOver);
      Assert.Equal(player, sut.PlayerOnTurn);
      grid.Verify(x => x.Drop(player, 0));
    }

    [Fact]
    public void Drop_WhenGridIsFull() {
      Player player = Player.Yellow;
      var grid = new Mock<Grid>();
      grid.Setup(x => x.Drop(player, 0)).Returns(true);
      grid.Setup(x => x.HasFourInARow(player)).Returns(false);
      grid.Setup(x => x.Full).Returns(true);
      Game sut = new(grid.Object, player);

      sut.Drop(0);

      Assert.Equal(Player.None, sut.Winner);
      Assert.True(sut.GameOver);
      Assert.Equal(player, sut.PlayerOnTurn);
      grid.Verify(x => x.Drop(player, 0));
    }

    [Fact]
    public void Drop_WhenGameIsNotOver() {
      Player player = Player.Yellow;
      var grid = new Mock<Grid>();
      grid.Setup(x => x.Drop(player, 0)).Returns(true);
      grid.Setup(x => x.HasFourInARow(player)).Returns(false);
      grid.Setup(x => x.Full).Returns(false);
      Game sut = new(grid.Object, player);

      sut.Drop(0);

      Assert.Equal(Player.None, sut.Winner);
      Assert.False(sut.GameOver);
      Assert.Equal(Player.Red, sut.PlayerOnTurn);
      grid.Verify(x => x.Drop(player, 0));
    }
  }
}
