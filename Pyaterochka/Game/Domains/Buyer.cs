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
    private float GetSpeed() => isEscaping && isThief ? 16f : 8f;

    private List<Point> customPath = new();
    private bool useCustomPath = false;

    private bool isThief { get; set; }
    private int moveTimer = 0;
    private const int moveInterval = 15;
    private int step = 2;

    private bool isEscaping = false;
    private bool hasLeftShop = false;
    private const int minEscapeTime = 10 * 60;
    private const int maxEscapeTime = 20 * 60;
    
    private Vector2 currentTarget;
    private List<Point> pathToDoor = new();
    private bool movingToTarget = false;
    private int leaveTimer;
    private const int minLeaveTime = 20 * 60;
    private const int maxLeaveTime = 30 * 60;
    private int escapeMoveTimer = 0;
    private const int escapeMoveInterval = 15; 
    
    private int randomWanderTimer;
    private const int minWanderTime = 7 * 60;
    private const int maxWanderTime = 10 * 60;
    private List<Point> wanderPath = new();
    private bool isWandering = false;

    public Buyer(Vector2 startPosition, IPlayer player, bool isThief = false)
    {
        this.player = player;
        Position = startPosition;
        SnapToGrid(); 
        this.isThief = isThief;
        randomWanderTimer = random.Next(minWanderTime, maxWanderTime);
        if (isThief)
            escapeTimer = random.Next(minEscapeTime, maxEscapeTime);
        else
            leaveTimer = random.Next(minLeaveTime, maxLeaveTime);
    }

    public void Update(GameMap map)
    {
        if (IsBanned || hasLeftShop) return;

        if (isEscaping && movingToTarget)
        {
            escapeMoveTimer++; 
            if (escapeMoveTimer < escapeMoveInterval)
                return;

            escapeMoveTimer = 0; 
            
            if (IsAtCenterOfCell())
            {
                if (pathToDoor.Count > 0)
                {
                    currentTarget = PathPointToVector(pathToDoor[0]);
                    pathToDoor.RemoveAt(0);
                }
                else
                {
                    LeaveFromShop();
                    return;
                }
            }
            MoveToTarget(currentTarget);
            
            if (Vector2.Distance(Position, new Vector2(map.Door.X, map.Door.Y)) <= HitBox * 1.2)
            {
                LeaveFromShop();
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
        
        randomWanderTimer--;
        if (randomWanderTimer <= 0 && !movingToTarget)
        {
            StartRandomWander(map);
        }

        moveTimer++;
        if (moveTimer >= moveInterval)
        { 
            if (isWandering)
            {
                if (wanderPath.Count > 0)
                {
                    currentTarget = PathPointToVector(wanderPath[0]);
                    wanderPath.RemoveAt(0);
                    if (!TryMove(currentTarget, map))
                    {
                        ResetWander();
                    };
                }
                else
                {
                    ResetWander();
                }
                return;
            }
            var direction = GetValidDirection(map);

            if (direction == Vector2.Zero)
            {
                var nearestPoint = FindNearestEmptyPoint(map);
                MoveToTarget(PathPointToVector(nearestPoint));
            }
            moveTimer = 0;
        }
    }
    
    private bool IsAtCenterOfCell()
    {
        var gridX = (int)(Position.X / HitBox);
        var gridY = (int)(Position.Y / HitBox);

        var centerX = gridX * HitBox;
        var centerY = gridY * HitBox;

        return Math.Abs(Position.X - centerX) < 0.1f && Math.Abs(Position.Y - centerY) < 0.1f;
    }

    private void ResetWander()
    {
        isWandering = false;
        wanderPath.Clear();
        randomWanderTimer = random.Next(minWanderTime, maxWanderTime);
    }
    
    private void MoveToTarget(Vector2 target)
    {
        Position = target; 
    }
    

    public void Ban() => IsBanned = true;

    public bool IsThief() => isThief;

    public void LeaveFromShop()
    {
        hasLeftShop = true;
        Ban();
        if (isThief)
            player.TakeDamage(1);
    }
    
    private void StartRandomWander(GameMap map)
    {
        var start = new Point((int)(Position.X / HitBox), (int)(Position.Y / HitBox));

        Point target;
        do
        {
            var x = random.Next(GameMap.Map.GetLength(1));
            var y = random.Next(GameMap.Map.GetLength(0));
            target = new Point(x, y);
        } while (!IsValidPoint(GameMap.Map, target));

        wanderPath = BFS.FindPath(map, start, target);
        if (wanderPath.Count > 0)
        {
            isWandering = true;
            currentTarget = PathPointToVector(wanderPath[0]);
            wanderPath.RemoveAt(0);
            MoveToTarget(currentTarget);
        }
    }


    private Vector2 PathPointToVector(Point point)
    {
        return new Vector2(point.X * HitBox, point.Y * HitBox);
    }
    
    private List<Point> FindPathToDoor(GameMap map)
    {
        var start = new Point(
            (int)Math.Floor(Position.X / HitBox),
            (int)Math.Floor(Position.Y / HitBox)
        );
        var end = new Point(map.Door.X / HitBox, map.Door.Y / HitBox);
        var path =  BFS.FindPath(map, start, end);
        return path;
    }

    private bool IsValidPoint(int[,] map, Point point)
    {
        var width = map.GetLength(1);
        var height = map.GetLength(0);
        if (point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height)
            return false;
        return map[point.Y, point.X] == 0;
    }

    private void StartEscape(GameMap map)
    {
        var nearestEmptyPoint = FindNearestEmptyPoint(map);
        var targetPosition = PathPointToVector(nearestEmptyPoint);
        
        MoveToTarget(targetPosition);
        
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
        if (nextPosition.X < 0 || nextPosition.Y < 0 || 
            nextPosition.X >= GameMap.Map.GetLength(0) * HitBox || 
            nextPosition.Y >= GameMap.Map.GetLength(1) * HitBox)
            return false;

        var hitbox = new Rectangle((int)nextPosition.X, (int)nextPosition.Y, HitBox, HitBox);
        if (map.Walls.Any(w => w.Intersects(hitbox)))
            return false;

        Position = nextPosition;
        return true;
    }
        
    
    private Point FindNearestEmptyPoint(GameMap map)
    {
        var width = GameMap.Map.GetLength(1);
        var height = GameMap.Map.GetLength(0);

        var startX = (int)(Position.X / HitBox);
        var startY = (int)(Position.Y / HitBox);
        
        for (int radius = 1; radius < Math.Max(width, height); radius++)
        {
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    if (Math.Abs(dx) != radius && Math.Abs(dy) != radius)
                        continue; 

                    var checkX = startX + dx;
                    var checkY = startY + dy;

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                    {
                        var point = new Point(checkX, checkY);
                        if (IsValidPoint(GameMap.Map, point))
                        {
                            return point;
                        }
                    }
                }
            }
        }
        
        return new Point(startX, startY);
    }
    
    private void SnapToGrid()
    {
        var gridX = (int)(Position.X / HitBox);
        var gridY = (int)(Position.Y / HitBox);

        Position = new Vector2(gridX * HitBox, gridY * HitBox);
    }
}