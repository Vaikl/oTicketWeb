using Clinic.Interfaces;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public HomeController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PrefferedServices = _serviceRepository.PrefferedServices
            };

            return View(homeViewModel);
        }
    }
}