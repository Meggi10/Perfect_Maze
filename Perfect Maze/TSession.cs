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
        public static Modes Mode { get; set; } 
        public enum Modes { Speedrun, FogOfWar}
        public static void Clear()
        {
            PlayerName = null;
            Mode = default;
            Names.Clear();
        }
    }
}
