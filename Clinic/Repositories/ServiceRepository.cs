using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Clinic.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ApplicationIdentityDbContext _applicationIdentityDbContext;

        public ServiceRepository(
            ApplicationDbContext applicationDbContext,
            ApplicationIdentityDbContext applicationIdentityDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _applicationIdentityDbContext = applicationIdentityDbContext;
        }

        public IEnumerable<Service> Services => _applicationDbContext.Services.ToList();

        public IEnumerable<Service> GetAllServices() => Services.OrderBy(p => p.Name).ToList();

        public IEnumerable<Service> PrefferedServices => _applicationDbContext.Services.Where(p => p.IsPrefferedService).Include(c => c.Category);

        public Service GetServiceById(int serviceId) => _applicationDbContext.Services.FirstOrDefault(d => d.ServiceId == serviceId);

        public void SaveService(Service service)
        { 
           if (service != null && service.ServiceId == 0)
            {
               
                _applicationDbContext.Services.Add(service);
            }
            else
            {
                Service dbEntry = _applicationDbContext.Services.FirstOrDefault(d => d.ServiceId == service.ServiceId);

                if (service != null && dbEntry != null)
                {
                    dbEntry.Name = service.Name;
                    dbEntry.ShortDescription = service.ShortDescription;
                    dbEntry.LongDescription = service.LongDescription;
                    dbEntry.Price = service.Price;
                    dbEntry.DoctorId = service.DoctorId;
                    dbEntry.DoctorName = service.DoctorName;
                    dbEntry.ImageUrl = service.ImageUrl;
                    dbEntry.ImageThumbnailUrl = service.ImageThumbnailUrl;
                    dbEntry.IsPrefferedService = service.IsPrefferedService;
                    dbEntry.CategoryId = service.CategoryId;
                }
            }

            _applicationDbContext.SaveChanges();
        }

        public Service DeleteService(int serviceId)
        {
            Service dbEntry = _applicationDbContext.Services.FirstOrDefault(d => d.ServiceId == serviceId);

            Service serviceToDelete = _applicationDbContext.Services.Where(d => d.ServiceId == serviceId).SingleOrDefault();

            if (dbEntry != null)
            {
                _applicationDbContext.Services.Remove(dbEntry);
                _applicationDbContext.SaveChanges();
            }

            return dbEntry;
        }
    }
}