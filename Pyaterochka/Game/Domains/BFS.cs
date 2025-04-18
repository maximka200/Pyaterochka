using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pyaterochka
{
    public static class BFS
    {
        /// <summary>
        /// Находит путь от начальной точки до конечной на карте GameMap.
        /// </summary>
        /// <param name="gameMap">Объект GameMap, содержащий карту.</param>
        /// <param name="start">Начальная точка пути.</param>
        /// <param name="end">Конечная точка пути.</param>
        /// <returns>Список точек, представляющих путь. Если путь не найден, возвращается пустой список.</returns>
        public static List<Point> FindPath(GameMap gameMap, Point start, Point end)
        {
            var map = GameMap.Map;
            var width = map.GetLength(1);
            var height = map.GetLength(0);
    
            if (!IsValidPoint(map, start) || !IsValidPoint(map, end))
            {
                return new List<Point>();
            }
    
            var queue = new Queue<Point>();
            var parentMap = new Point?[height, width];
            var visited = new bool[height, width];
    
            queue.Enqueue(start);
            visited[start.Y, start.X] = true;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // Если достигли конечной точки, восстанавливаем путь
                if (current.Equals(end))
                {
                    return ReconstructPath(parentMap, start, end);
                }

                // Обрабатываем соседние точки
                foreach (var neighbor in GetNeighbors(current, width, height))
                {
                    if (IsValidPoint(map, neighbor) && !visited[neighbor.Y, neighbor.X])
                    {
                        queue.Enqueue(neighbor);
                        parentMap[neighbor.Y, neighbor.X] = current;
                        visited[neighbor.Y, neighbor.X] = true;
                    }
                }
            }

            // Если путь не найден, возвращаем пустой список
            return new List<Point>();
        }


        /// <summary>
        /// Восстанавливает путь из массива родителей.
        /// </summary>
        /// <param name="parentMap">Массив, хранящий родителей для каждой точки.</param>
        /// <param name="start">Начальная точка пути.</param>
        /// <param name="end">Конечная точка пути.</param>
        /// <returns>Список точек, представляющих путь.</returns>
        private static List<Point> ReconstructPath(Point?[,] parentMap, Point start, Point end)
        {
            List<Point> path = new List<Point>();
            Point? current = end;

            while (current != null)
            {
                path.Add(current.Value);
                current = parentMap[current.Value.Y, current.Value.X];
            }

            path.Reverse();
            return path;
        }

        /// <summary>
        /// Проверяет, является ли точка валидной (проходимой и внутри границ карты).
        /// </summary>
        /// <param name="map">Массив карты.</param>
        /// <param name="point">Точка для проверки.</param>
        /// <returns>True, если точка валидна; иначе False.</returns>
        private static bool IsValidPoint(int[,] map, Point point)
        {
            var width = map.GetLength(1);
            var height = map.GetLength(0);

            if (point.X < 0 || point.X >= width || point.Y < 0 || point.Y >= height)
            {
                return false;
            }

            return map[point.Y, point.X] == 0 || map[point.Y, point.X] == 2; // Проходимая клетка
        }

        /// <summary>
        /// Возвращает соседние точки для заданной точки.
        /// </summary>
        /// <param name="point">Текущая точка.</param>
        /// <param name="width">Ширина карты.</param>
        /// <param name="height">Высота карты.</param>
        /// <returns>Перечисление соседних точек.</returns>
        private static IEnumerable<Point> GetNeighbors(Point point, int width, int height)
        {
            int[][] directions =
            {
                new[] { 0, 1 },  // Вниз
                new[] { 1, 0 },  // Вправо
                new[] { 0, -1 }, // Вверх
                new[] { -1, 0 }  // Влево
            };

            foreach (var dir in directions)
            {
                var newX = point.X + dir[0];
                var newY = point.Y + dir[1];

                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    yield return new Point(newX, newY);
                }
            }
        }
        public static void PrintMapWithPath(int[,] map, List<Point> path, Point? start = null, Point? end = null)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);
            var pathSet = new HashSet<Point>(path);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var p = new Point(x, y);

                    if (start.HasValue && p == start.Value)
                    {
                        Console.Write('S'); // Start
                    }
                    else if (end.HasValue && p == end.Value)
                    {
                        Console.Write('E'); // End
                    }
                    else if (pathSet.Contains(p))
                    {
                        Console.Write('*'); // Path
                    }
                    else
                    {
                        switch (map[y, x])
                        {
                            case 0:
                                Console.Write('.'); // Проходимая клетка
                                break;
                            case 1:
                                Console.Write('#'); // Стена
                                break;
                            case 2:
                                Console.Write('D'); // Дверь
                                break;
                            default:
                                Console.Write('?'); // Что-то непонятное
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }

    }
}