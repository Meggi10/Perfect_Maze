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
