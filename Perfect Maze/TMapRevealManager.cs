using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TMapRevealManager
    {
        private bool[,] DiscoveredCells;
        private int r;
        private int n;
        public bool IsVisible(int x, int y) => DiscoveredCells[x, y];
        
        public TMapRevealManager(int n, int r)
        {
            this.n = n;
            this.r = r;
            DiscoveredCells = new bool[n, n];
        }

        public void RevealArea(int posX, int posY, TCell[,] cells)
        {
            for (int x = 0; x < n; x++)
                for (int y = 0; y < n; y++)
                {
                    double distance = Math.Sqrt(Math.Pow(x - posX, 2) + Math.Pow(y - posY, 2));
                    if (distance > r)
                        continue;
                    if (HasLineOfSight(posX, posY, x, y, cells))
                        DiscoveredCells[x, y] = true;
                }
        }
        private bool HasLineOfSight(int x0, int y0, int x1, int y1, TCell[,] cells)
        {
            var line = TAlgorithm.Bresenham(x0, y0, x1, y1);
            for (int i = 0; i < line.Count - 1; i++)
            {
                TCell current = cells[line[i].X, line[i].Y];
                TCell next = cells[line[i + 1].X, line[i + 1].Y];
                if (!current.Connected.Contains(next))
                    return false;
            }
            return true;
        }
    }
}
