using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pyaterochka;

public class Buyer : IBuyer
{
    public Vector2 Position { get; private set; }
    public bool IsBanned { get; private set; }
    public int HitBox => 40;
    protected IPlayer player;
    protected static Random random = new Random();
    protected int escapeTimer;
    private float GetSpeed() => isEscaping && isThief ? 8f : 4f;

    private List<Point> customPath = new();
    private bool useCustomPath = false;

    private bool isThief { get; set; }
    private int moveTimer = 0;
    private const int moveInterval = 5;
    private int step = 2;

    private bool isEscaping = false;
    private bool hasLeftShop = false;
    private const int minEscapeTime = 0 * 60;
    private const int maxEscapeTime = 4 * 60;
    
    private Vector2 currentTarget;
    private List<Point> pathToDoor = new();
    private bool movingToTarget = false;
    private int leaveTimer;
    private const int minLeaveTime = 20 * 60;
    private const int maxLeaveTime = 30 * 60;

    public Buyer(Vector2 startPosition, IPlayer player, bool isThief = false)
    {
        this.player = player;
        Position = startPosition;
        this.isThief = isThief;

        if (isThief)
            escapeTimer = random.Next(minEscapeTime, maxEscapeTime);
        else
            leaveTimer = random.Next(minLeaveTime, maxLeaveTime);
    }

    public void Update(GameMap map)
    {
        if (IsBanned || hasLeftShop) return;

       /* if (useCustomPath && customPath.Count > 0)
        {
            MoveTowards(currentTarget, map);
            if (Vector2.Distance(Position, currentTarget) < GetSpeed())
            {
                Position = currentTarget;
                if (customPath.Count > 0)
                {
                    currentTarget = PathPointToVector(customPath[0]);
                    customPath.RemoveAt(0);
                }
                else
                {
                    useCustomPath = false;
                }
            }
            return;
        }

        if (!useCustomPath)
            TryStartCustomPath(map);
        */
        if (isEscaping && movingToTarget)
        {
            MoveTowards(currentTarget, map);

            if (Vector2.Distance(Position, new Vector2(map.Door.X, map.Door.Y)) <= HitBox * 1.2)
            {
                LeaveFromShop();
                return;
            }

            if (Vector2.Distance(Position, currentTarget) < GetSpeed())
            {
                Position = currentTarget;
                if (pathToDoor.Count > 0)
                {
                    currentTarget = PathPointToVector(pathToDoor[0]);
                    pathToDoor.RemoveAt(0);
                }
                else
                {
                    LeaveFromShop();
                }
            }
            return;
        }
       
        if (isThief && !isEscaping)
        {
            escapeTimer--;
            if (escapeTimer <= 0)
            {
                isEscaping = true;
                StartEscape(map);
            }
        }
        else if (!isEscaping)
        {
            leaveTimer--;
            if (leaveTimer <= 0)
            {
                isEscaping = true;
                StartEscape(map);
            }
        }

        moveTimer++;
        if (moveTimer >= moveInterval)
        {
            Vector2 direction = GetValidDirection(map);
            moveTimer = 0;
        }
    }

    public void Ban() => IsBanned = true;

    public bool IsThief() => isThief;

    public virtual void LeaveFromShop()
    {
        hasLeftShop = true;
        Ban();
        if (isThief)
            player.TakeDamage(1);
    }

    private void TryStartCustomPath(GameMap map)
    {
        customPath = FindRandomPath(map);
        if (customPath.Count > 0)
        {
            useCustomPath = true;
            currentTarget = PathPointToVector(customPath[0]);
            customPath.RemoveAt(0);
        }
    }

    private Vector2 PathPointToVector(Point point)
    {
        return new Vector2(point.X * HitBox + HitBox / 2, point.Y * HitBox + HitBox / 2);
    }

    private List<Point> FindRandomPath(GameMap map)
    {
        var width = map.Map.GetLength(1);
        var height = map.Map.GetLength(0);
        var start = new Point((int)Position.X / HitBox, (int)Position.Y / HitBox);

        if (!IsValidPoint(map.Map, start)) return new();

        Point end;
        int attempts = 0;
        do
        {
            end = new Point(random.Next(width), random.Next(height));
            attempts++;
        } while ((!IsValidPoint(map.Map, end) || end == start) && attempts < 100);

        if (attempts >= 100) return new();

        return BFS.FindPath(map, start, end);
    }

    private List<Point> FindPathToDoor(GameMap map)
    {
        var start = new Point(
            (int)Math.Floor(Position.X / HitBox),
            (int)Math.Floor(Position.Y / HitBox)
        );
        var end = new Point(map.Door.X / HitBox, map.Door.Y / HitBox);
        var path =  BFS.FindPath(map, start, end);
        BFS.PrintMapWithPath(new int[,]
        {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1},
            {2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        }, path, start, end);
        return path;
    }

    private bool IsValidPoint(int[,] map, Point point)
    {
        var width = map.GetLength(1);
        var height = map.GetLength(0);
        if (point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height)
            return false;
        return map[point.Y, point.X] == 0 || map[point.Y, point.X] == 2;
    }

    private void StartEscape(GameMap map)
    {
        pathToDoor = FindPathToDoor(map);
        if (pathToDoor.Count == 0)
        {
            isEscaping = false;
            escapeTimer = 60;
            return;
        }
        currentTarget = PathPointToVector(pathToDoor[0]);
        pathToDoor.RemoveAt(0);
        movingToTarget = true;
    }

    private Vector2 GetValidDirection(GameMap map)
    {
        Vector2[] directions =
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1)
        };

        var shuffled = directions.OrderBy(_ => random.Next()).ToArray();

        foreach (var dir in shuffled)
        {
            if (TryMove(dir, map))
                return dir;
        }

        return Vector2.Zero;
    }

    private bool TryMove(Vector2 direction, GameMap map)
    {
        var nextPosition = Position + direction * GetSpeed();
        var hitbox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox, HitBox);

        if (map.Walls.Any(w => w.Intersects(hitbox)))
            return false;

        Position = nextPosition;
        return true;
    }

    private bool CanMove(Vector2 direction, GameMap map)
    {
        var nextPosition = Position + direction * GetSpeed();
        var hitbox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox, HitBox);
        return !map.Walls.Any(w => w.Intersects(hitbox));
    }

    private void MoveTowards(Vector2 target, GameMap map)
    {
        var toTarget = target - Position;
        if (toTarget.Length() == 0) return;

        var direction = Vector2.Normalize(toTarget);
        TryMove(direction, map);
    }
    
    private Point FindNearestEmptyPoint(GameMap map)
    {
        var width = map.Map.GetLength(1);
        var height = map.Map.GetLength(0);

        int startX = (int)(Position.X / HitBox);
        int startY = (int)(Position.Y / HitBox);

        // Проверяем все соседние клетки в порядке увеличения расстояния
        for (int radius = 0; radius < Math.Max(width, height); radius++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                        continue; // Пропускаем внутренние клетки

                    int checkX = startX + dx;
                    int checkY = startY + dy;

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                    {
                        var point = new Point(checkX, checkY);
                        if (IsValidPoint(map.Map, point))
                        {
                            return point;
                        }
                    }
                }
            }
        }

        // Если не найдено свободных клеток, возвращаем текущую позицию
        return new Point(startX, startY);
    }
}