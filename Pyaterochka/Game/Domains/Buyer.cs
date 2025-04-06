using System;
using Microsoft.Xna.Framework;

namespace Pyaterochka;

public class Buyer : IBuyer
{
    public Vector2 Position { get; private set; }
    public bool IsBanned { get; private set; }
    public int HitBox => 90;
    private bool isThief;
    private static Random random = new Random();
    private Vector2 direction;
    private int moveTimer = 0;
    private const int moveInterval = 60; // раз в 60 апдейтов меняем направление
    private const float speed = 1.5f;

    public Buyer(Vector2 startPosition, bool isThief = false)
    {
        Position = startPosition;
        this.isThief = isThief;
        direction = GetRandomDirection();
    }

    public void Update(Rectangle[] walls, Rectangle door)
    {
        moveTimer++;
        if (moveTimer >= moveInterval)
        {
            direction = GetRandomDirection();
            moveTimer = 0;
        }

        var newPosition = Position + direction * speed;
        var newBounds = new Rectangle((int)newPosition.X, (int)newPosition.Y, HitBox, HitBox);
        var collides = false;

        foreach (var wall in walls)
        {
            if (newBounds.Intersects(wall) || newBounds.Intersects(door))
            {
                collides = true;
                break;
            }
        }

        if (!collides)
        {
            Position = newPosition;
        }
        else
        {
            direction = GetRandomDirection();
        }
    }
    
    public void Ban()
    {
        IsBanned = true;
    }

    private Vector2 GetRandomDirection()
    {
        var dx = random.Next(-1, 2);
        var dy = random.Next(-1, 2);
        return new Vector2(dx, dy);
    }

    public bool IsThief()
    {
        return isThief;
    }

    public void LeaveFromShop()
    {
        throw new NotImplementedException();
    }

    public void SetThief(bool value)
    {
        isThief = value;
    }
}