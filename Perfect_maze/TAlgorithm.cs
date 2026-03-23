using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TAlgorithm
    {
        public static TCell BFS(TCell root, TCell goal)
        {
            var queue = new Queue<TCell>();
            var explored = new HashSet<TCell>();
            explored.Add(root);
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
    }
}
