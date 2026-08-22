using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public static class THelpers
    {
        public static string GetDisplayName(TSession.Modes mode)
        {
            switch (mode)
            {
                case TSession.Modes.Speedrun:
                    return "Speedrun";
                case TSession.Modes.FogOfWar:
                    return "Fog Of War";
                default:
                    return mode.ToString();
            }
        }
    }
}
