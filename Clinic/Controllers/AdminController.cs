using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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
        private ApplicationDbContext _applicationDbContext;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdminController(IServiceRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            repository = repo;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index()
        {
            ClaimsPrincipal user = this.User;
            string idDoctor = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            string[] roles = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToArray();
            if (roles.Contains("Admin"))
            {
                 return View(repository.Services);
            }
            else
            {
                IEnumerable<Service> services = repository.GetAllServices(idDoctor);
                return View(services);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int serviceId)
        {
            Service service = repository.Services.FirstOrDefault(p => p.ServiceId == serviceId);
            List<Object> doctors = new List<Object>();
            List<Object> categorys = new List<Object>();
            foreach (ApplicationUser us in _userManager.Users)
            {
                List<string> roles = (List<string>)Task.Run(() => _userManager.GetRolesAsync(us)).Result;
                if (roles.Contains("Doctor"))
                {
                    doctors.Add(new { Id = us.Id, UserName = us.FirstName + " " + us.LastName });
                }
            }

            foreach (Category ca in _applicationDbContext.Categories)
            {
                categorys.Add(new { Id = ca.CategoryId, Name = ca.Name });
            }
            ViewBag.Categorys = new SelectList(categorys, "Id", "Name", service.CategoryId );
            ViewBag.Doctors = new SelectList(doctors, "Id", "UserName", service.DoctorId);
            return View(service);
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Service service)
        {
            ClaimsPrincipal user = this.User;
            string idDoctor = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            string[] roles = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToArray();
         
            foreach (var error in ModelState)
            {
                if(error.Key.Equals("DoctorId")){
                    ModelState.Remove(error.Key);
                }
               
            }
            if (!roles.Contains("Admin"))
            {
                service.DoctorId = idDoctor;
                ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == service.DoctorId).FirstOrDefault();
                service.DoctorName = applicationUser.FirstName + " " + applicationUser.LastName;
            }
            else
            {
           
                service.DoctorId =Request.Form["ddlist"].ToString();
                ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == service.DoctorId).FirstOrDefault();
                service.DoctorName = applicationUser.FirstName + " " + applicationUser.LastName;
            }
           
           service.CategoryId = Int32.Parse(Request.Form["calist"].ToString());     
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
            List<Object> doctors = new List<Object>();
            List<Object> categorys = new List<Object>();
            foreach (ApplicationUser us in _userManager.Users)
            {
                List<string> roles = (List<string>)Task.Run(() => _userManager.GetRolesAsync(us)).Result;
                if (roles.Contains("Doctor"))
                {
                    doctors.Add(new {Id = us.Id, UserName=us.FirstName+ " "+us.LastName });
                }
            } 

            foreach (Category ca in _applicationDbContext.Categories)
            {
                categorys.Add(new {Id = ca.CategoryId, Name = ca.Name });
            }
            ViewBag.Categorys = new SelectList(categorys, "Id", "Name");
            ViewBag.Doctors = new SelectList(doctors, "Id", "UserName");        
            return  View("Edit", new Service());
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Delete(int serviceId)
        {
            List<ShoppingCartItem> shoppingCartItems = _applicationDbContext.ShoppingCartItems.Where(x => x.Service.ServiceId == serviceId).ToList();
            foreach (ShoppingCartItem shoppingCart in shoppingCartItems)
            {
                _applicationDbContext.ShoppingCartItems.Remove(shoppingCart);
            }
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