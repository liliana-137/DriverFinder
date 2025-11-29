using System;
using System.Collections.Generic;
using System.Text;

namespace DriverFinder
{
    using System.Collections.Generic;

    public interface IDriverService
    {
        void AddOrUpdateDriver(Driver driver);
        void RemoveDriver(string driverId);
        Driver GetDriver(string driverId);
        IEnumerable<Driver> GetAllDrivers();
        void Clear();
    }
}
