﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class GameModel
{
    public GameMap Map { get; private set; } // Заменено int[,] на GameMap
    public int TileSize => 40;

    public IPlayer Player { get; private set; }
    public List<IBuyer> Buyers { get; private set; } = new();
    public bool IsGameOver => (Player?.Health ?? 3) < 1;
    public Rectangle[] Walls => Map.Walls;
    public Rectangle Door => Map.Door;

    private bool isGameOverHandled = false;
    private double gameOverTimer = 0;

    private BuyerSpawner spawner;

    public GameModel()
    {
        Player = new Player(new Vector2(100, 100));
        Map = new GameMap(); // Инициализация карты
        
        var random = new Random();
        Buyers.Add(new Buyer(new Vector2(300, 200), Player, random.NextDouble() < 0.2));
        
        spawner = new BuyerSpawner(Buyers, Map.Door, Player); // Передаем дверь из карты
    }

    public void Update(GameTime gameTime)
    {
        Player.Update(Walls, Door); // Передаем стену и дверь из карты

        foreach (var buyer in Buyers.ToArray()) // ToArray чтобы безопасно удалять
        {
            buyer.Update(Walls, Door);

            if (buyer.IsBanned)
                Buyers.Remove(buyer);
        }

        spawner.Update();

        if (IsGameOver)
        {
            gameOverTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (!isGameOverHandled)
            {
                isGameOverHandled = true;
                gameOverTimer = 0;
            }
            else if (gameOverTimer > 3)
            {
                Environment.Exit(0);
            }
        }
    }
    
}
