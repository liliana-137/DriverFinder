using DriverFinder;
using System.Collections.Generic;
using System.Linq;

public class KDTreeDriverFinder : IDriverFinder
{
    private readonly IDriverService _driverService;
    private List<Driver> _drivers; // Простая замена

    public string AlgorithmName => "K-D Tree";

    public KDTreeDriverFinder(IDriverService driverService)
    {
        _driverService = driverService;
    }

    public List<DriverSearchResult> FindNearestDrivers(int targetX, int targetY, int count = 5)
    {
        // Упрощенная версия - используем линейный поиск вместо K-D Tree
        _drivers = _driverService.GetAllDrivers().ToList();

        if (_drivers.Count == 0)
            return new List<DriverSearchResult>();

        var results = new List<DriverSearchResult>();

        foreach (var driver in _drivers)
        {
            double distance = driver.DistanceTo(targetX, targetY);
            results.Add(new DriverSearchResult(driver, distance));
        }

        return results.OrderBy(r => r.Distance).Take(count).ToList();
    }
}