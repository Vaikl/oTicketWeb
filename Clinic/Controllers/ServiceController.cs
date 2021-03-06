using Clinic.Interfaces;
using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Clinic.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ServiceController(IServiceRepository serviceRepository, ICategoryRepository categoryRepository)
        {
            _serviceRepository = serviceRepository;
            _categoryRepository = categoryRepository;
        }

        public ViewResult List(string category)
        {
            string _category = category;
            IEnumerable<Service> services;
            string currentCategory = string.Empty;

             if (string.IsNullOrEmpty(category))
            {
                List<Category> categories = _categoryRepository.Categories.ToList();
                services = _serviceRepository.Services.Where(p => p.Category != null).OrderBy(p => p.Name);
                currentCategory = "All services";
            }
            else
            {
                  int categoryId = _categoryRepository.Categories.Where(x => x.Name.Equals(category)).First().CategoryId;
                    services = _serviceRepository.Services.Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name);
                    currentCategory = _category;
            }

            return View(new ServicesListViewModel
            {
                Services = services,
                CurrentCategory = currentCategory
            });
        }

        public ViewResult Search(string searchString)
        {
            string _searchString = searchString;
            IEnumerable<Service> services;
            string currentCategory = string.Empty;

            if (string.IsNullOrEmpty(_searchString))
            {
                List<Category> categories = _categoryRepository.Categories.ToList();
                services = _serviceRepository.Services.Where(p => p.Category != null).OrderBy(p => p.Name);
            }
            else
            {
                List<Category> categories = _categoryRepository.Categories.ToList();
                services = _serviceRepository.Services.Where(p => p.Category != null).OrderBy(p => p.Name).Where(p => p.Name.ToLower().Contains(_searchString.ToLower()));
            }

            return View("~/Views/Service/List.cshtml", new ServicesListViewModel { Services = services, CurrentCategory = "All" });
        }

        public ViewResult Details(int serviceId)
        {
            var service = _serviceRepository.Services.FirstOrDefault(p => p.ServiceId == serviceId);

            if (service == null)
            {
                return View("~/Views/Error/Error.cshtml");
            }

            return View(service);
        }
    }
}