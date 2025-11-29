using DriverFinder;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class DriverFinderTests
{
    private IDriverService _driverService;
    private List<IDriverFinder> _finders;

    [SetUp]
    public void Setup()
    {
        _driverService = new DriverService();
        _finders = new List<IDriverFinder>
        {
            new LinearSearchDriverFinder(_driverService),
            new PriorityQueueDriverFinder(_driverService),
            new SpatialPartitionDriverFinder(_driverService),
            new KDTreeDriverFinder(_driverService)
        };
    }

    [Test]
    public void FindNearestDrivers_WithMultipleDrivers_ReturnsCorrectResults()
    {
        // Arrange
        _driverService.AddOrUpdateDriver(new Driver("d1", 0, 0));
        _driverService.AddOrUpdateDriver(new Driver("d2", 10, 10));
        _driverService.AddOrUpdateDriver(new Driver("d3", 20, 20));
        _driverService.AddOrUpdateDriver(new Driver("d4", 30, 30));
        _driverService.AddOrUpdateDriver(new Driver("d5", 40, 40));

        int targetX = 5, targetY = 5;

        foreach (var finder in _finders)
        {
            // Act
            var results = finder.FindNearestDrivers(targetX, targetY, 3);

            // Assert
            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results[0].Driver.Id, Is.EqualTo("d1")); // Closest
            Assert.That(results[1].Driver.Id, Is.EqualTo("d2"));
            Assert.That(results[2].Driver.Id, Is.EqualTo("d3"));
        }
    }

    [Test]
    public void FindNearestDrivers_WithFewerDriversThanRequested_ReturnsAllDrivers()
    {
        // Arrange
        _driverService.AddOrUpdateDriver(new Driver("d1", 0, 0));
        _driverService.AddOrUpdateDriver(new Driver("d2", 10, 10));

        foreach (var finder in _finders)
        {
            // Act
            var results = finder.FindNearestDrivers(0, 0, 5);

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
        }
    }

    [Test]
    public void FindNearestDrivers_WithNoDrivers_ReturnsEmptyList()
    {
        // Arrange - no drivers added

        foreach (var finder in _finders)
        {
            // Act
            var results = finder.FindNearestDrivers(0, 0, 5);

            // Assert
            Assert.That(results, Is.Empty);
        }
    }

    [Test]
    public void FindNearestDrivers_WithSameLocation_ReturnsCorrectOrder()
    {
        // Arrange
        _driverService.AddOrUpdateDriver(new Driver("d1", 5, 5));
        _driverService.AddOrUpdateDriver(new Driver("d2", 5, 5));
        _driverService.AddOrUpdateDriver(new Driver("d3", 10, 10));

        foreach (var finder in _finders)
        {
            // Act
            var results = finder.FindNearestDrivers(5, 5, 2);

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.That(results[0].Distance, Is.EqualTo(0));
            Assert.That(results[1].Distance, Is.EqualTo(0));
        }
    }

    [Test]
    public void FindNearestDrivers_DistanceCalculation_IsCorrect()
    {
        // Arrange
        _driverService.AddOrUpdateDriver(new Driver("d1", 0, 0));
        _driverService.AddOrUpdateDriver(new Driver("d2", 3, 4)); // Distance should be 5

        foreach (var finder in _finders)
        {
            // Act
            var results = finder.FindNearestDrivers(0, 0, 2);

            // Assert
            Assert.That(results[0].Distance, Is.EqualTo(0));
            Assert.That(results[1].Distance, Is.EqualTo(5));
        }
    }
}