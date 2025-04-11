using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class GameModel
{
    public int[,] Map { get; private set; }
    public int TileSize => 40;

    public IPlayer Player { get; private set; }
    public IBuyer[] Buyers { get; private set; }
    public bool IsGameOver => (Player?.Health ?? 3) < 1;
    public Rectangle[] walls => GetWallsFromMap();
    public Rectangle[] doors;

    private bool isGameOverHandled = false;
    private double gameOverTimer = 0;

    public GameModel()
    {
        Player = new Player(new Vector2(100, 100));
        Buyers = new Buyer[]
        {
            new Buyer(new Vector2(300, 200), Player, true),
        };

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
    }

    public void Update(GameTime gameTime)
    {
        Player.Update(GetWallsFromMap(), GetDoorFromMap());
        foreach (var buyer in Buyers)
        {
            buyer.Update(GetWallsFromMap(), GetDoorFromMap());
        }

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
