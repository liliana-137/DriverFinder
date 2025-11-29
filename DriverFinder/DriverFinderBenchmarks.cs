using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.Collections.Generic;

    [MemoryDiagnoser]
    [RankColumn]
    public class DriverFinderBenchmarks
    {
        private IDriverService _driverService;
        private List<IDriverFinder> _finders;
        private readonly Random _random = new Random(42);

        [Params(100, 1000)]
        public int DriverCount { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _driverService = new DriverService();

            for (int i = 0; i < DriverCount; i++)
            {
                var driver = new Driver(
                    $"driver_{i}",
                    _random.Next(0, 1000),
                    _random.Next(0, 1000)
                );
                _driverService.AddOrUpdateDriver(driver);
            }

            _finders = new List<IDriverFinder>
        {
            new LinearSearchDriverFinder(_driverService),
            new PriorityQueueDriverFinder(_driverService),
            new SpatialPartitionDriverFinder(_driverService, 50),
            new KDTreeDriverFinder(_driverService)
        };
        }

        [Benchmark]
        public void LinearSearch() => RunBenchmark(0);

        [Benchmark]
        public void Priorit() => RunBenchmark(1);

        [Benchmark]
        public void SpatialPartition() => RunBenchmark(2);

        [Benchmark]
        public void KDTree() => RunBenchmark(3);

        private void RunBenchmark(int finderIndex)
        {
            var targetX = _random.Next(0, 1000);
            var targetY = _random.Next(0, 1000);
            var result = _finders[finderIndex].FindNearestDrivers(targetX, targetY, 5);
        }
    }
}
