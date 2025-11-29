using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using DriverFinder;

public class Program
{
    public static void Main(string[] args)
    {
        // Для бенчмарков с отключением валидации оптимизаций
        var config = DefaultConfig.Instance
            .WithOptions(ConfigOptions.DisableOptimizationsValidator);

        BenchmarkRunner.Run<DriverFinderBenchmarks>(config);

        // Для демо (если нужно вернуть потом):
        // DemoDriverFinder();
    }

    private static void DemoDriverFinder()
    {
        var driverService = new DriverService();

        driverService.AddOrUpdateDriver(new Driver("d1", 10, 20));
        driverService.AddOrUpdateDriver(new Driver("d2", 30, 40));
        driverService.AddOrUpdateDriver(new Driver("d3", 50, 60));
        driverService.AddOrUpdateDriver(new Driver("d4", 70, 80));
        driverService.AddOrUpdateDriver(new Driver("d5", 90, 100));
        driverService.AddOrUpdateDriver(new Driver("d6", 15, 25));

        var finders = new List<IDriverFinder>
        {
            new LinearSearchDriverFinder(driverService),
            new PriorityQueueDriverFinder(driverService),
            new SpatialPartitionDriverFinder(driverService),
            new KDTreeDriverFinder(driverService)
        };

        int targetX = 25, targetY = 35;

        foreach (var finder in finders)
        {
            Console.WriteLine($"\nAlgorithm: {finder.AlgorithmName}");
            var results = finder.FindNearestDrivers(targetX, targetY, 3);

            foreach (var result in results)
            {
                Console.WriteLine($"Driver {result.Driver.Id}: ({result.Driver.X}, {result.Driver.Y}) - Distance: {result.Distance:F2}");
            }
        }
    }
}