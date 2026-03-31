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
        public static TCell BFS(TCell root, TCell goal)
        {
            var queue = new Queue<TCell>();
            var explored = new HashSet<TCell>() { root };
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var visited = queue.Dequeue();
                if (visited == goal)
                    return visited;
                foreach (var neighbour in visited.Connected)
                {
                    if (!explored.Contains(neighbour))
                    {
                        explored.Add(neighbour);
                        neighbour.Parent = visited;
                        queue.Enqueue(neighbour);
                    }
                }
            }
            return null;
        }
        public static List<TCell> GetPath(TCell goalCell)
        {
            var path = new List<TCell>();
            var current = goalCell;
            while (current != null)
            {
                path.Add(current);
                current = current.Parent;
            }
            path.Reverse();
            return path;
        }
        public static List<TCell> AStar(TCell root, TCell goal)
        {
            var cellsToCheck = new HashSet<TCell> { root };
            var cellsChecked = new HashSet<TCell>();
            var stepFromStart = new Dictionary<TCell, double> { [root] = 0 };
            while (cellsToCheck.Count > 0)
            {
                TCell current = null;
                double lowestF = double.MaxValue;
                foreach (var node in cellsToCheck)
                {
                    double g = stepFromStart.ContainsKey(node) ? stepFromStart[node] : double.MaxValue;
                    double heuristic = Math.Abs(root.X - goal.X) + Math.Abs(root.Y - goal.Y);
                    double f = g + heuristic;
                    if (f < lowestF)
                    {
                        lowestF = f;
                        current = node;
                    }
                }
                cellsToCheck.Remove(current);
                cellsChecked.Add(current);
                if (current == goal)
                    return GetPath(goal);
                foreach (var neighbour in current.Connected)
                {
                    if (cellsChecked.Contains(neighbour))
                        continue;
                    double stepCost = stepFromStart[current] + 1;
                    var gg = stepFromStart.ContainsKey(neighbour) ? stepFromStart[neighbour] : double.MaxValue;
                    if (stepCost < gg || !cellsToCheck.Contains(neighbour))
                    {
                        neighbour.Parent = current;
                        stepFromStart[neighbour] = stepCost;
                        if (!cellsToCheck.Contains(neighbour))
                            cellsToCheck.Add(neighbour);
                    }
                }
            }
            return null;
        }
    }
}
