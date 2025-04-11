using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class GameModel
{
    public int[,] Map { get; private set; }
    public int TileSize => 40;

    public IPlayer Player { get; private set; }
    public List<IBuyer> Buyers { get; private set; } = new();
    public bool IsGameOver => (Player?.Health ?? 3) < 1;
    public Rectangle[] Walls => GetWallsFromMap();

    private bool isGameOverHandled = false;
    private double gameOverTimer = 0;

    private BuyerSpawner spawner;

    public GameModel()
    {
        Player = new Player(new Vector2(100, 100));

        Map = new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
        };
        var random = new Random();
        Buyers.Add(new Buyer(new Vector2(300, 200), Player, random.NextDouble() < 0.2));
        
        spawner = new BuyerSpawner(Buyers, GetDoorFromMap(), Player);
    }

    public void Update(GameTime gameTime)
    {
        Player.Update(Walls, GetDoorFromMap());

        foreach (var buyer in Buyers.ToArray()) // ToArray чтобы безопасно удалять
        {
            buyer.Update(Walls, GetDoorFromMap());

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

    public Rectangle[] GetWallsFromMap()
    {
        var walls = new List<Rectangle>();
        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                if (Map[y, x] == 1)
                {
                    walls.Add(new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize));
                }
            }
        }
        return walls.ToArray();
    }

    public Rectangle GetDoorFromMap()
    {
        for (int y = 0; y < Map.GetLength(0); y++)
        {
            for (int x = 0; x < Map.GetLength(1); x++)
            {
                if (Map[y, x] == 2)
                {
                    return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
                }
            }
        }
        return Rectangle.Empty;
    }
}
