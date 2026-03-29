using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TCell
    {
        public int X, Y;
        public List<TCell> Connected = new List<TCell>();
        public TCell Parent { get; set; }
    }
}
