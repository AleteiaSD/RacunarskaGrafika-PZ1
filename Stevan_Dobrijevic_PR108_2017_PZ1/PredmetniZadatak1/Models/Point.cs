using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak1.Models
{
    public class Point
    {
        private double x;
        private double y;
        public Point()
        {

        }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
    }
}
