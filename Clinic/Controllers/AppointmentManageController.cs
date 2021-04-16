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
        public AppointmentManageController(IAppointmentRepository repo, IHttpContextAccessor accessor, ApplicationDbContext applicationDbContext)
        {
            repository = repo;
            httpContextAccessor = accessor;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Appointments);

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


            ViewBag.Digs = new SelectList(digs, "Id", "Name");
         
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
    }
}