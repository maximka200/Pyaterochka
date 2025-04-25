using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pyaterochka.Buyers;

namespace Pyaterochka;

public class BuyerSpawner
{
    private readonly List<IBuyer> buyers;
    private readonly Rectangle door;
    private readonly IPlayer player;
    private readonly Random random = new();
    private int spawnTimer = 0;
    private const int spawnInterval = 60 * 20;

    public BuyerSpawner(List<IBuyer> buyers, Rectangle door, IPlayer player)
    {
        this.buyers = buyers;
        this.door = door;
        this.player = player;
    }

    public void Update()
    {
        spawnTimer++;
        if (spawnTimer >= spawnInterval)
        {
            SpawnBuyer();
            spawnTimer = 0;
        }
    }

    private void SpawnBuyer()
    {
        var spawnPos = new Vector2(door.X, door.Y + 40);
        IBuyer buyer;
        var rand = random.Next(3);
        if (rand == 0)
        {
            buyer = new Boozer(spawnPos, player);
        }
        else if (rand == 1)
        {
            buyer = new Babushka(spawnPos, player);
        }
        else
        {
            buyer = new Usual(spawnPos, player);
        }
    
        buyers.Add(buyer);
    }
}