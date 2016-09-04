using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadCrosser
{
    public class Road
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsSafe { get; set; }

        public Road(int id, int x, int y, int height, int width, bool isSafe)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
            this.IsSafe = IsSafe;
        }
    }
}
