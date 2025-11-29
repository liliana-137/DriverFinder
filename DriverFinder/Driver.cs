using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    public class Driver
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Driver(string id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public double DistanceTo(int targetX, int targetY)
        {
            int dx = X - targetX;
            int dy = Y - targetY;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
