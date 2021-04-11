using Clinic.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Clinic.Interfaces
{
    public interface IServiceRepository
    {
        IEnumerable<Service> Services { get; }
        IEnumerable<Service> PrefferedServices { get; }

        IEnumerable<Service> GetAllServices();

        void SaveService(Service service);

        Service DeleteService(int serviceId);

        Service GetServiceById(int serviceId);

        IEnumerable<Service> GetAllServices(string idDoctor);
    }
}