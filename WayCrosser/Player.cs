using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadCrosser
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        //public Road Road { get; set; }
        public int RoadId { get; set; }

        public Player(int x, int y, int roadId)
        {
            this.Y = y;
            this.X = x;
            this.RoadId = roadId;
        }

    }
}
