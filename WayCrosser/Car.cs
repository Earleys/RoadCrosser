using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadCrosser
{
    public class Car
    {
        public Image Image { get; set; }
        public Road Road { get; set; }
        public int X { get; set; }
        public int Length { get; set; }

        public Car(Image image, Road road, int x, int length)
        {
            this.Image = image;
            this.Road = road;
            this.X = x;
            this.Length = length;
        }
    }
}
