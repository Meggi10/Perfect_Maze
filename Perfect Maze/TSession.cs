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
        public static Modes Mode { get; set; }
        public static DiffLvls DifficultyLvl { get; set; }
        public enum Modes { Speedrun, FogOfWar}
        public enum DiffLvls { Easy, Normal, Hard}
        public static void Clear()
        {
            PlayerName = null;
            Mode = default;
            DifficultyLvl = default;
        }
    }
}
