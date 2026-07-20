using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TAlgorithm
    {
        public static List<TCell> BFS(TCell root, TCell goal)
        {
            var queue = new Queue<TCell>();
            var explored = new HashSet<TCell>() { root };
            var cameFrom = new Dictionary<TCell, TCell>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var visited = queue.Dequeue();
                if (visited == goal)
                    return ReconstructPath(cameFrom, goal);
                foreach (var neighbour in visited.Connected)
                {
                    if (!explored.Contains(neighbour))
                    {
                        explored.Add(neighbour);
                        cameFrom[neighbour] = visited;
                        queue.Enqueue(neighbour);
                    }
                }
            }
            return null;
        }
        public static List<TCell> AStar(TCell root, TCell goal)
        {
            var cellsToCheck = new HashSet<TCell> { root };
            var cellsChecked = new HashSet<TCell>();
            var stepFromStart = new Dictionary<TCell, double> { [root] = 0 };
            var cameFrom = new Dictionary<TCell, TCell>();
            while (cellsToCheck.Count > 0)
            {
                TCell current = null;
                double lowestF = double.MaxValue;
                foreach (var node in cellsToCheck)
                {
                    double g = stepFromStart[node];
                    double heuristic = Math.Abs(node.X - goal.X) + Math.Abs(node.Y - goal.Y);
                    double f = g + heuristic;
                    if (f < lowestF)
                    {
                        lowestF = f;
                        current = node;
                    }
                }
                if (current == goal)
                    return ReconstructPath(cameFrom, goal);
                cellsToCheck.Remove(current);
                cellsChecked.Add(current);
                foreach (var neighbour in current.Connected)
                {
                    if (cellsChecked.Contains(neighbour))
                        continue;

                    double stepCost = stepFromStart[current] + 1;
                    double previousCost = stepFromStart.ContainsKey(neighbour) ? stepFromStart[neighbour] : double.MaxValue;

                    if (stepCost < previousCost)
                    {
                        cameFrom[neighbour] = current;
                        stepFromStart[neighbour] = stepCost;

                        if (!cellsToCheck.Contains(neighbour))
                            cellsToCheck.Add(neighbour);
                    }
                }
            }
            return null;
        }
        public static List<TCell> GenerationMazeDFS(TCell[,] cells, int n, TCell startCell, Random rnd)
        {
            var depthCells = new List<TCell>();
            var path = new List<TCell>();
            var actCell = startCell;
            var directions = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            depthCells.Add(actCell);
            while (depthCells.Count > 0)
            {
                path.Add(actCell);
                var freeCells = new List<TCell>();
                foreach (var (dirX, dirY) in directions)
                {
                    int neighX = actCell.X + dirX;
                    int neighY = actCell.Y + dirY;
                    if (neighX >= 0 && neighX < n && neighY >= 0 && neighY < n)
                    {
                        var neighbour = cells[neighX, neighY];
                        if (neighbour.Connected.Count == 0)
                        {
                            freeCells.Add(neighbour);
                        }
                    }
                }
                if (freeCells.Count > 0)
                {
                    var neighbour = freeCells[rnd.Next(freeCells.Count)];
                    actCell.Connected.Add(neighbour);
                    neighbour.Connected.Add(actCell);
                    depthCells.Add(actCell);
                    actCell = neighbour;
                }
                else
                {
                    actCell = depthCells.Last();
                    depthCells.RemoveAt(depthCells.Count - 1);
                }
            }
            return path;
        }
        private static List<TCell> ReconstructPath(Dictionary<TCell, TCell> cameFrom, TCell goal)
        {
            var path = new List<TCell> { goal };
            var current = goal;
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }
    }
}
