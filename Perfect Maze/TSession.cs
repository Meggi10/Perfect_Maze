using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public static class TSession
    {
        public static string PlayerName { get; set; }
        public static List<string> Names { get; set; } = new List<string>();
        public static void Clear()
        {
            PlayerName = null;
            Names.Clear();
        }
    }
}
