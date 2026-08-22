using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfect_maze
{
    public class TPlayerSession
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public Mode Mode { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
