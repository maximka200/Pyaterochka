using NUnit.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Pyaterochka;

[TestFixture]
public class GameModelTests
{
    [Test]
    public void GameModel_InitializesCorrectly()
    {
        var model = new GameModel();

        Assert.IsNotNull(model.Player);
        Assert.IsNotNull(model.Map);
        Assert.IsFalse(model.IsGameOver);
    }

    [Test]
    public void GameModel_GameOver_WhenHealthIsZero()
    {
        var model = new GameModel();
        model.Player.Health = 0;

        Assert.IsTrue(model.IsGameOver);
    }

    [Test]
    public void GameModel_RemovesBannedBuyers()
    {
        var model = new GameModel();
        var testBuyer = new TestBuyer { Banned = true };
        model.Buyers.Add(testBuyer);

        model.Update(new GameTime());

        Assert.IsFalse(model.Buyers.Contains(testBuyer));
    }

    private class TestBuyer : IBuyer
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public int HitBox => 20;
        public bool IsBanned => Banned;
        public bool Banned { get; set; } = false;

        public void Update(GameMap map) {}
        public bool IsThief() => true;
        public void Ban() {}
    }
}