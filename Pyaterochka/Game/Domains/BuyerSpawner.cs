﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pyaterochka;

public class BuyerSpawner
{
    private readonly List<IBuyer> buyers;
    private readonly Rectangle door;
    private readonly IPlayer player;
    private readonly Random random = new();
    private int spawnTimer = 0;
    private const int spawnInterval = 60 * 12;

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
        Vector2 spawnPos = new Vector2(door.X , door.Y - 60);
        var isThief = random.NextDouble() < 0.2;
        buyers.Add(new Buyer(spawnPos, player, isThief));
    }
}