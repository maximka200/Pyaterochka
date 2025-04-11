using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class Buyer : IBuyer
{
    public Vector2 Position { get; private set; }
    public bool IsBanned { get; private set; }
    public int HitBox => 48;

    private bool isThief;
    private static Random random = new Random();
    private int moveTimer = 0;
    private const int moveInterval = 20;
    private const float speed = 1.5f;

    private bool isEscaping = false;
    private int escapeTimer;
    private bool hasLeftShop = false;

    private Rectangle currentDoor;
    private Vector2 currentTarget;
    private List<Point> pathToDoor = new();

    private bool movingToTarget = false;
    private IPlayer player;

    public Buyer(Vector2 startPosition, IPlayer player, bool isThief = false)
    {
        this.player = player;
        Position = startPosition;
        this.isThief = isThief;

        if (isThief)
        {
            escapeTimer = random.Next(5 * 60, 15 * 60);
        }
    }

    public void Update(Rectangle[] walls, Rectangle door)
    {
        if (IsBanned || hasLeftShop) return;

        currentDoor = door;

        if (isThief && !isEscaping)
        {
            escapeTimer--;
            if (escapeTimer <= 0)
            {
                isEscaping = true;
                pathToDoor = FindPathToDoor(walls, door);
                if (pathToDoor.Count > 0)
                {
                    currentTarget = new Vector2(pathToDoor[0].X * HitBox, pathToDoor[0].Y * HitBox);
                    movingToTarget = true;
                    pathToDoor.RemoveAt(0);
                }
            }
        }

        if (isEscaping && movingToTarget)
        {
            Vector2 toTarget = currentTarget - Position;
            if (toTarget.Length() < speed)
            {
                Position = currentTarget;

                if (pathToDoor.Count > 0)
                {
                    currentTarget = new Vector2(pathToDoor[0].X * HitBox, pathToDoor[0].Y * HitBox);
                    pathToDoor.RemoveAt(0);
                }
                else
                {
                    LeaveFromShop();
                }
            }
            else
            {
                Vector2 direction = Vector2.Normalize(toTarget);
                Position += direction * speed;
            }

            return;
        }

        // обычное поведение
        moveTimer++;
        if (moveTimer >= moveInterval)
        {
            Vector2 direction = GetRandomDirection();
            var newPos = Position + direction * 10;
            Position = newPos;
            moveTimer = 0;
        }
    }

    public void Ban() => IsBanned = true;

    public bool IsThief() => isThief;

    public void LeaveFromShop()
    {
        hasLeftShop = true;
        Ban();
        if (this.isThief)
        {
            player.TakeDamage(1);
        }
    }

    public bool HasLeft => hasLeftShop;

    private Vector2 GetRandomDirection()
    {
        Vector2[] directions =
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
        };

        return directions[random.Next(directions.Length)];
    }

    private List<Point> FindPathToDoor(Rectangle[] walls, Rectangle door)
    {
        int width = 20;  // ширина карты в клетках
        int height = 15; // высота карты
        bool[,] map = new bool[width, height];

        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            map[x, y] = true;

        foreach (var wall in walls)
        {
            int wx = wall.X / HitBox;
            int wy = wall.Y / HitBox;
            for (int dx = 0; dx < wall.Width / HitBox; dx++)
            for (int dy = 0; dy < wall.Height / HitBox; dy++)
            {
                if (wx + dx < width && wy + dy < height)
                    map[wx + dx, wy + dy] = false;
            }
        }

        Point start = new Point((int)Position.X / HitBox, (int)Position.Y / HitBox);
        Point end = new Point(door.Center.X / HitBox, door.Center.Y / HitBox);

        return AStar.FindPath(map, start, end);
    }
}
