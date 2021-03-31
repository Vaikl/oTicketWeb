using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Clinic.Controllers
{
    public class AdminController : Controller
    {
        private IServiceRepository repository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(IServiceRepository repo)
        {
            repository = repo;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Services);

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int serviceId) => View(repository.Services.FirstOrDefault(p => p.ServiceId == serviceId));

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                repository.SaveService(service);
                log.Info($"Диагноз {service.Name} отредактирован или создан.");
                TempData["message"] = $"{service.Name} был сохранен";
                return RedirectToAction("Index");
            }
            else
            {
                return View(service);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Create() => View("Edit", new Service());

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Delete(int serviceId)
        {
            Service deletedService = repository.DeleteService(serviceId);
            log.Info($"Диагноз {deletedService} удален.");
            if (deletedService != null)
            {
                TempData["message"] = $"{deletedService.Name} был удален";
            }
            return RedirectToAction("Index");
        }
    }
}