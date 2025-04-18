using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pyaterochka;

public class GameMap
{
    public readonly static int[,] Map = new int[,]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1},
        {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
        {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
    };
    public int TileSize => 40;
    public Rectangle[] Walls => GetWallsFromMap();
    public Rectangle Door => GetDoorFromMap();

    public GameMap()
    {
    }

    private Rectangle[] GetWallsFromMap()
    {
        var walls = new List<Rectangle>();
        for (var y = 0; y < Map.GetLength(0); y++)
        {
            for (var x = 0; x < Map.GetLength(1); x++)
            {
                if (Map[y, x] == 1)
                {
                    walls.Add(new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize));
                }
            }
        }
        return walls.ToArray();
    }

    private Rectangle GetDoorFromMap()
    {
        for (var y = 0; y < Map.GetLength(0); y++)
        {
            for (var x = 0; x < Map.GetLength(1); x++)
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