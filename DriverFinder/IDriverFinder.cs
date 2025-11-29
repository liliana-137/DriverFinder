using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using System.Collections.Generic;

    public interface IDriverFinder
    {
        List<DriverSearchResult> FindNearestDrivers(int targetX, int targetY, int count = 5);
        string AlgorithmName { get; }
    }

}