using NUnit.Framework;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Pyaterochka;

[TestFixture]
public class BuyerTests
{
    private TestPlayer player;
    private GameMap map;

    [SetUp]
    public void Setup()
    {
        player = new TestPlayer();
        map = new GameMap();
    }

    [Test]
    public void Buyer_IsThief_ReturnsTrueIfConstructedAsThief()
    {
        var thief = new Buyer(new Vector2(40, 40), player, isThief: true);
        Assert.IsTrue(thief.IsThief());
    }

    [Test]
    public void Buyer_Ban_SetsIsBannedTrue()
    {
        var buyer = new Buyer(new Vector2(40, 40), player);
        Assert.IsFalse(buyer.IsBanned);

        buyer.Ban();

        Assert.IsTrue(buyer.IsBanned);
    }

    [Test]
    public void Buyer_LeaveFromShop_SetsIsBannedTrue()
    {
        var buyer = new Buyer(new Vector2(40, 40), player);
        buyer.LeaveFromShop();

        Assert.IsTrue(buyer.IsBanned);
    }

    [Test]
    public void Buyer_ThiefLeaves_TakesDamageFromPlayer()
    {
        var thief = new Buyer(new Vector2(40, 40), player, isThief: true);
        Assert.AreEqual(0, player.DamageTaken);

        thief.LeaveFromShop();

        Assert.AreEqual(1, player.DamageTaken);
    }

    [Test]
    public void Buyer_Update_WhenBanned_DoesNothing()
    {
        var buyer = new Buyer(new Vector2(40, 40), player);
        buyer.Ban();

        var originalPosition = buyer.Position;

        for (int i = 0; i < 100; i++)
            buyer.Update(map);

        Assert.AreEqual(originalPosition, buyer.Position);
    }
}

public class TestPlayer : IPlayer
{
    public Vector2 Position { get; set; }
    public int Health { get; set; } = 3;
    public int Stamina { get; set; } = 100;
    public int Score { get; set; } = 0;
    public int HitBox => 40;

    public int DamageTaken { get; private set; } = 0;

    public void TakeDamage(int amount)
    {
        DamageTaken += amount;
        Health -= amount;
    }

    public void Update(GameMap map) { }
}
