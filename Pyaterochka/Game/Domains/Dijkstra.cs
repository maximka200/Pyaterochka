using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pyaterochka
{
    public static class Dijkstra
    {
        public static List<Point> FindPath(bool[,] map, int[,] cost, Point start, Point end)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);

            var openSet = new SortedSet<Node>(Comparer<Node>.Create((a, b) =>
                a.Cost == b.Cost ? a.Position.GetHashCode().CompareTo(b.Position.GetHashCode()) : a.Cost.CompareTo(b.Cost)));
            var cameFrom = new Dictionary<Point, Point>();
            var gScore = new Dictionary<Point, int>();
            var fScore = new Dictionary<Point, int>();
            
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var point = new Point(x, y);
                    gScore[point] = int.MaxValue;
                    fScore[point] = int.MaxValue;
                }
            }

            gScore[start] = 0;
            fScore[start] = Heuristic(start, end);
            openSet.Add(new Node { Position = start, Cost = fScore[start] });

            while (openSet.Count > 0)
            {
                var current = openSet.Min;
                openSet.Remove(current);

                if (current.Position == end)
                {
                    return ReconstructPath(cameFrom, current.Position);
                }

                foreach (var neighbor in GetNeighbors(current.Position, map))
                {
                    int tentativeGScore = gScore[current.Position] + 1;
                    if (tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current.Position;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, end);
                        
                        if (!openSet.Contains(new Node { Position = neighbor }))
                        {
                            openSet.Add(new Node { Position = neighbor, Cost = fScore[neighbor] });
                        }
                    }
                }
            }

            return new List<Point>();
        }

        private static List<Point> GetNeighbors(Point point, bool[,] map)
        {
            var neighbors = new List<Point>();
            var x = point.X;
            var y = point.Y;

            if (x > 0 && map[x - 1, y]) neighbors.Add(new Point(x - 1, y));
            if (x < map.GetLength(0) - 1 && map[x + 1, y]) neighbors.Add(new Point(x + 1, y));
            if (y > 0 && map[x, y - 1]) neighbors.Add(new Point(x, y - 1));
            if (y < map.GetLength(1) - 1 && map[x, y + 1]) neighbors.Add(new Point(x, y + 1));

            return neighbors;
        }

        private static int Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y); 
        }

        private static List<Point> ReconstructPath(Dictionary<Point, Point> cameFrom, Point current)
        {
            var totalPath = new List<Point> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }

        private class Node
        {
            public Point Position { get; set; }
            public int Cost { get; set; }
        }
    }
}
