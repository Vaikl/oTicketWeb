using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    public class AdminController : Controller
    {
        private IServiceRepository repository;
        private UserManager<ApplicationUser> _userManager;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(IServiceRepository repo, UserManager<ApplicationUser> userManager)
        {
            repository = repo;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Services);

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int serviceId) => View(repository.Services.FirstOrDefault(p => p.ServiceId == serviceId));

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Service service)
        {
            ClaimsPrincipal user = this.User;
            string idDoctor = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            string nameDoctor = user.Identity.Name;
            string[] roles = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToArray();
            service.DoctorName = nameDoctor;
            foreach (var error in ModelState)
            {
                if(error.Key.Equals("DoctorId")){
                    ModelState.Remove(error.Key);
                }
               
            }
            if (!roles.Contains("Admin"))
            {
                service.DoctorId = idDoctor;
            }
            else
            {
                service.DoctorId =Request.Form["ddlist"].ToString();
                service.DoctorName = _userManager.Users.Where(x => x.Id == service.DoctorId).FirstOrDefault().UserName;
            }
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
        public ViewResult Create()
        {
            List<ApplicationUser> list = new List<ApplicationUser>();
            foreach (ApplicationUser us in _userManager.Users)
            {
                List<string> roles = (List<string>)Task.Run(() => _userManager.GetRolesAsync(us)).Result;
                if (roles.Contains("Doctor"))
                {
                    list.Add(us);
                }
            }
            ViewBag.Doctors = new SelectList(list, "Id", "UserName");        
            return  View("Edit", new Service());
        }

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