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
    
        if (random.Next(2) == 0)
        {
            buyer = new Boozer(spawnPos, player);
        }
        else
        {
            buyer = new Babushka(spawnPos, player);
        }
    
        buyers.Add(buyer);
    }
}