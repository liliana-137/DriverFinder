using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    public class DriverSearchResult
    {
        public Driver Driver { get; set; }
        public double Distance { get; set; }

        public DriverSearchResult(Driver driver, double distance)
        {
            Driver = driver;
            Distance = distance;
        }
    }
}
