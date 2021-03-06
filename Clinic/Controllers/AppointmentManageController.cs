using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Clinic.Controllers
{
    public class AppointmentManageController : Controller
    {
        private readonly IAppointmentRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private ApplicationDbContext _applicationDbContext;
        private UserManager<ApplicationUser> _userManager;
        public AppointmentManageController(IAppointmentRepository repo, IHttpContextAccessor accessor, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            repository = repo;
            httpContextAccessor = accessor;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index(string name)
        {
            if (name != null) return View(repository.Appointments.Where(x => x.PatientFullName.Contains(name)));
          return View(repository.Appointments);
        }

        [Authorize(Roles = "Patient")]
        public ViewResult MyAppointments() => View(repository.Appointments
            .Where(a => a.PatientId == httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int appointmentId)
        {
            List<Object> digs = new List<Object>();

            foreach (Diagnosis diagnosis in _applicationDbContext.Diagnoses)
            {
                digs.Add(new { Id = diagnosis.DiagnosisId ,Name = diagnosis.Name });
            }


            ViewBag.Digs = new SelectList(digs, "Id", "Name", repository.Appointments.FirstOrDefault(p => p.AppointmentId == appointmentId).DiagnosisId);
         
            return View(repository.Appointments.FirstOrDefault(p => p.AppointmentId == appointmentId));
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Appointment appointment)
        {
            appointment = _applicationDbContext.Appointments.FirstOrDefault(x => x.AppointmentId == appointment.AppointmentId);
            appointment.DiagnosisId = Int32.Parse(Request.Form["Digs"].ToString());
            appointment.DiagnosName = _applicationDbContext.Diagnoses.FirstOrDefault(x => x.DiagnosisId == appointment.DiagnosisId).Name;
            repository.SaveAppointment(appointment);
                return RedirectToAction("Index");
          
        }

      

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Delete(int appointmentId)
        {
            Appointment deletedAppoint = repository.DeleteAppointment(appointmentId);
          
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin, Doctor")]
        public IActionResult Done(int appointmentId)
        {
            Appointment deletedAppoint = repository.DeleteAppointment(appointmentId);
            ClaimsPrincipal user = this.User;
            string idDoctor = user.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == idDoctor).FirstOrDefault();
            _applicationDbContext.AppointmentsDone.Add(new AppointmentDone()
            {
                AppointmentPlaced = DateTime.Now,
                DiagnosisId = deletedAppoint.DiagnosisId,
                DiagnosName = deletedAppoint.DiagnosName,
                DoctorId = applicationUser.FirstName + " " + applicationUser.LastName,
                PatientFullName = deletedAppoint.PatientFullName,
                PatientId = deletedAppoint.PatientId,
                TotalSum = deletedAppoint.TotalSum
            });
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}