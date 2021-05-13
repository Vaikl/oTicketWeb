using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    public class PrescriptionController : Controller
    {
        private IPrescriptionRepository repository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _applicationDbContext;
        public PrescriptionController(IPrescriptionRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            repository = repo;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int prescriptionId)
        {
            Prescription prescription = repository.Prescriptions.FirstOrDefault(p => p.PrescriptionId == prescriptionId);
            createViewBag(prescription.PatientName);
            return View(prescription);
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                prescription.PatientName = Request.Form["users"].ToString();
                prescription.PrescriptionDate = DateTime.Now;
                repository.SavePrescription(prescription);
                log.Info($"Диагноз {prescription.PrescriptionId} отредактирован или создан.");
                TempData["message"] = $"{prescription.PrescriptionId} был сохранен";
                return RedirectToAction("Index");
            }
            else
            {
                createViewBag(prescription.PatientName);
                return View(prescription);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Create()
        {
            createViewBag(null);
          return View("Edit", new Prescription());
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ActionResult Index(string name)
        {
            if (name == null) return View(repository.Prescriptions);
            else
            return View(repository.Prescriptions.Where(x => x.PatientName.Contains(name)));
        }

        private void createViewBag(string name)
        {
            List<Object> users = new List<Object>();
           
            foreach (ApplicationUser us in _userManager.Users)
            {
                List<string> roles = (List<string>)Task.Run(() => _userManager.GetRolesAsync(us)).Result;
                if (roles.Contains("Patient"))
                {
                    users.Add(new { UserName = us.FirstName + " " + us.LastName });
                }
            }

           
            ViewBag.Users = new SelectList(users, "UserName", "UserName", name);
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Delete(int prescriptionId)
        {
            Prescription persc = _applicationDbContext.Prescriptions.FirstOrDefault(x => x.PrescriptionId == prescriptionId);
            if (persc != null) _applicationDbContext.Prescriptions.Remove(persc);
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}