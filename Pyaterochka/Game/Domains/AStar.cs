using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public static class AStar
{
    private class Node
    {
        public Point Position;
        public Node Parent;
        public int G, H;
        public int F => G + H;
    }

    public static List<Point> FindPath(bool[,] map, Point start, Point end)
    {
        var open = new List<Node>();
        var closed = new HashSet<Point>();
        open.Add(new Node { Position = start, G = 0, H = Heuristic(start, end) });

        while (open.Count > 0)
        {
            open.Sort((a, b) => a.F.CompareTo(b.F));
            var current = open[0];
            open.RemoveAt(0);
            closed.Add(current.Position);

            if (current.Position == end)
            {
                var path = new List<Point>();
                while (current != null)
                {
                    path.Add(current.Position);
                    current = current.Parent;
                }

                path.Reverse();
                return path;
            }

            foreach (var offset in new Point[]
            {
                new Point(1, 0), new Point(-1, 0),
                new Point(0, 1), new Point(0, -1)
            })
            {
                var neighborPos = current.Position + offset;
                if (neighborPos.X < 0 || neighborPos.Y < 0 ||
                    neighborPos.X >= map.GetLength(0) || neighborPos.Y >= map.GetLength(1))
                    continue;

                if (!map[neighborPos.X, neighborPos.Y] || closed.Contains(neighborPos))
                    continue;

                int g = current.G + 1;
                var existing = open.Find(n => n.Position == neighborPos);
                if (existing == null)
                {
                    open.Add(new Node
                    {
                        Position = neighborPos,
                        G = g,
                        H = Heuristic(neighborPos, end),
                        Parent = current
                    });
                }
                else if (g < existing.G)
                {
                    existing.G = g;
                    existing.Parent = current;
                }
            }
        }

        return new List<Point>(); // путь не найден
    }

    private static int Heuristic(Point a, Point b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); // Manhattan
    }
}
