using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using System.Collections.Generic;

    public class DriverService : IDriverService
    {
        private readonly Dictionary<string, Driver> _drivers = new Dictionary<string, Driver>();

        public void AddOrUpdateDriver(Driver driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            _drivers[driver.Id] = driver;
        }

        public void RemoveDriver(string driverId)
        {
            _drivers.Remove(driverId);
        }

        public Driver GetDriver(string driverId)
        {
            return _drivers.TryGetValue(driverId, out var driver) ? driver : null;
        }

        public IEnumerable<Driver> GetAllDrivers()
        {
            return _drivers.Values;
        }

        public void Clear()
        {
            _drivers.Clear();
        }
    }
}