using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using System.Collections.Generic;
    using System.Linq;

    public class LinearSearchDriverFinder : IDriverFinder
    {
        private readonly IDriverService _driverService;

        public string AlgorithmName => "Linear Search with Sorting";

        public LinearSearchDriverFinder(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public List<DriverSearchResult> FindNearestDrivers(int targetX, int targetY, int count = 5)
        {
            var allDrivers = _driverService.GetAllDrivers();
            var results = new List<DriverSearchResult>();

            foreach (var driver in allDrivers)
            {
                double distance = driver.DistanceTo(targetX, targetY);
                results.Add(new DriverSearchResult(driver, distance));
            }

            return results.OrderBy(r => r.Distance).Take(count).ToList();
        }
    }
}
