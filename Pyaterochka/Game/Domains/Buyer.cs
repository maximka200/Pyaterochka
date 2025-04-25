using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class Buyer : IBuyer
{
    public bool IsThief()
    {
        return isThief;
    }

    public Vector2 Position { get; private set; }
    public bool IsBanned { get; private set; }
    public int HitBox => 40;
    protected IPlayer player;
    protected static Random random = new Random();
    
    private int leaveTimer;
    private const int minLeaveTime = 20 * 60;
    private const int maxLeaveTime = 30 * 60;
    
    private List<Point> currentPath = new();
    private Vector2 currentTarget;
    private bool hasLeftShop = false;
    private bool isThief;
    private const int thiefLeaveTime = 10 * 60;
    private const float walkSpeed = 1f;
    private const float escapeSpeed = 1.5f;
    
    public Buyer(Vector2 startPosition, IPlayer player, bool isThief = false)
    {
        this.player = player;
        this.isThief = isThief;
        Position = startPosition;
        SnapToGrid();
        
        // Для вора устанавливаем специальное время
        leaveTimer = isThief 
            ? thiefLeaveTime 
            : random.Next(minLeaveTime, maxLeaveTime);
    }

    public void Update(GameMap map)
    {
        if (IsBanned || hasLeftShop) return;

        leaveTimer--;

        if (leaveTimer > 0)
        {
            WanderBehavior(map);
        }
        else
        {
            EscapeBehavior(map);
        }
    }

    private void WanderBehavior(GameMap map)
    {
        if (currentPath.Count == 0 || IsAtPosition(currentTarget))
        {
            var start = CurrentCell();
            Point target;
        
            do
            {
                target = new Point(
                    random.Next(GameMap.Map.GetLength(1)),
                    random.Next(GameMap.Map.GetLength(0))
                );
            } while (!IsValidPoint(GameMap.Map, target));

            currentPath = BFS.FindPath(map, start, target);
            if (currentPath.Count == 0) return;

            // Пропускаем стартовую точку, если она есть в пути
            if (currentPath.Count > 0 && currentPath[0] == start)
            {
                currentPath.RemoveAt(0);
                if (currentPath.Count == 0) return;
            }
        
            currentTarget = PathPointToVector(currentPath[0]);
            currentPath.RemoveAt(0);
        }

        MoveTowardsTarget(map, false);
    }

    private void EscapeBehavior(GameMap map)
    {
        if (currentPath.Count == 0)
        {
            var path = FindPathToDoor(map);
            if (path.Count == 0)
            {
                LeaveFromShop();
                return;
            }
            currentPath = path;
            currentTarget = PathPointToVector(currentPath[0]);
            currentPath.RemoveAt(0);
        }

        if (IsAtPosition(currentTarget))
        {
            if (currentPath.Count > 0)
            {
                currentTarget = PathPointToVector(currentPath[0]);
                currentPath.RemoveAt(0);
            }
            else
            {
                LeaveFromShop();
                return;
            }
        }

        MoveTowardsTarget(map, true);
    }

    private void MoveTowardsTarget(GameMap map, bool escape)
    {
        Vector2 direction = Vector2.Normalize(currentTarget - Position);
        if (float.IsNaN(direction.X))
        {
            direction = Vector2.Zero;
        }
        
        var speed = escape ? walkSpeed : escapeSpeed;
        
        if (!TryMove(direction * speed, map))
        {
            currentPath.Clear();
        }
    }

    private bool IsAtPosition(Vector2 target)
    {
        return Vector2.Distance(Position, target) < 4f;
    }

    private Point CurrentCell()
    {
        return new Point(
            (int)(Position.X / HitBox),
            (int)(Position.Y / HitBox)
        );
    }

    private bool TryMove(Vector2 direction, GameMap map)
    {
        Vector2 newPosition = Position + direction;
        newPosition = Vector2.Clamp(newPosition, Vector2.Zero, 
            new Vector2(GameMap.Map.GetLength(1) * HitBox, GameMap.Map.GetLength(0) * HitBox));

        Rectangle hitbox = new Rectangle((int)newPosition.X, (int)newPosition.Y, HitBox, HitBox);
        if (map.Walls.Any(w => w.Intersects(hitbox)))
            return false;

        Position = newPosition;
        return true;
    }

    private List<Point> FindPathToDoor(GameMap map)
    {
        var start = CurrentCell();
        var end = new Point(map.Door.X / HitBox, map.Door.Y / HitBox);
        return BFS.FindPath(map, start, end);
    }

    public void Ban() => IsBanned = true;

    public void LeaveFromShop()
    {
        hasLeftShop = true;
        Ban();
        if (isThief)
        {
            player.TakeDamage(1);
        }
    }

    private Vector2 PathPointToVector(Point point)
    {
        return new Vector2(point.X * HitBox, point.Y * HitBox);
    }

    private bool IsValidPoint(int[,] map, Point point)
    {
        return point.X >= 0 && point.Y >= 0 && 
               point.X < map.GetLength(1) && 
               point.Y < map.GetLength(0) && 
               map[point.Y, point.X] == 0;
    }

    private void SnapToGrid()
    {
        Position = new Vector2(
            (int)(Position.X / HitBox) * HitBox,
            (int)(Position.Y / HitBox) * HitBox
        );
    }
}