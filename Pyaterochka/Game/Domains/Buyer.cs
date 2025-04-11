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
    private const float speed = 5f;

    private bool isEscaping = false;
    private int escapeTimer;
    private bool hasLeftShop = false;

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

        if (isThief && !isEscaping)
        {
            escapeTimer--;
            if (escapeTimer <= 0)
            {
                isEscaping = true;
                StartEscape(walls, door);
            }
        }

        if (isEscaping && movingToTarget)
        {
            MoveToTarget();
        }
        else
        {
            moveTimer++;
            if (moveTimer >= moveInterval)
            {
                Vector2 direction = GetValidDirection(walls);
                Position += direction * speed;
                moveTimer = 0;
            }
        }
    }

    private Vector2 GetValidDirection(Rectangle[] walls)
    {
        Vector2 direction;
        Rectangle futureHitbox;
        var attempts = 0;
        
        do
        {
            direction = GetRandomDirection();
            futureHitbox = new Rectangle(
                (int)(Position.X + direction.X * speed),
                (int)(Position.Y + direction.Y * speed),
                HitBox,
                HitBox
            );
            attempts++;
        } 
        while (attempts < 10 && CheckWallCollision(futureHitbox, walls));

        return direction;
    }

    private bool CheckWallCollision(Rectangle futureHitbox, Rectangle[] walls)
    {
        foreach (var wall in walls)
        {
            if (futureHitbox.Intersects(wall))
                return true;
        }
        return false;
    }

    private void StartEscape(Rectangle[] walls, Rectangle door)
    {
        pathToDoor = FindPathToDoor(walls, door);
        if (pathToDoor.Count > 0)
        {
            currentTarget = new Vector2(pathToDoor[0].X * HitBox, pathToDoor[0].Y * HitBox);
            pathToDoor.RemoveAt(0);
            movingToTarget = true;
        }
    }

    private void MoveToTarget()
    {
        var toTarget = currentTarget - Position;
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
        var width = 20;  
        var height = 15; 
        var cost = new int[width, height];
        var map = new bool[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                map[x, y] = true;
                cost[x, y] = int.MaxValue;
            }
        }
        
        foreach (var wall in walls)
        {
            var wx = wall.X / HitBox;
            var wy = wall.Y / HitBox;
            for (var dx = 0; dx < wall.Width / HitBox; dx++)
            {
                for (var dy = 0; dy < wall.Height / HitBox; dy++)
                {
                    if (wx + dx < width && wy + dy < height)
                        map[wx + dx, wy + dy] = false; // непроходимо
                }
            }
        }
        
        var start = new Point((int)Position.X / HitBox, (int)Position.Y / HitBox);
        var end = new Point(door.Center.X / HitBox, door.Center.Y / HitBox);
        
        return Dijkstra.FindPath(map, cost, start, end);
    }
}
