using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Clinic.Controllers
{
    public class AppointmentManageController : Controller
    {
        private readonly IAppointmentRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AppointmentManageController(IAppointmentRepository repo, IHttpContextAccessor accessor)
        {
            repository = repo;
            httpContextAccessor = accessor;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Appointments);

        [Authorize(Roles = "Patient")]
        public ViewResult MyAppointments() => View(repository.Appointments
            .Where(a => a.PatientId == httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int appointmentId) => View(repository.Appointments.FirstOrDefault(p => p.AppointmentId == appointmentId));

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                repository.SaveAppointment(appointment);
                return RedirectToAction("Index");
            }
            else
            {
                return View(appointment);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Create() => View("Edit", new Appointment());
    }
}