using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using System.Collections.Generic;
    using System.Linq;

    public class PriorityQueueDriverFinder : IDriverFinder
    {
        private readonly IDriverService _driverService;

        public string AlgorithmName => "Priority Queue";

        public PriorityQueueDriverFinder(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public List<DriverSearchResult> FindNearestDrivers(int targetX, int targetY, int count = 5)
        {
            var allDrivers = _driverService.GetAllDrivers();

            // Используем SortedDictionary вместо PriorityQueue для простоты
            var sortedResults = new SortedDictionary<double, List<DriverSearchResult>>();

            foreach (var driver in allDrivers)
            {
                double distance = driver.DistanceTo(targetX, targetY);
                var result = new DriverSearchResult(driver, distance);

                // Добавляем небольшое значение к расстоянию чтобы ключи были уникальными
                double uniqueKey = distance;
                while (sortedResults.ContainsKey(uniqueKey))
                {
                    uniqueKey += 0.000001;
                }

                sortedResults[uniqueKey] = new List<DriverSearchResult> { result };
            }

            return sortedResults.Values.SelectMany(list => list).Take(count).ToList();
        }
    }
}
