using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Clinic.Controllers
{
    public class PrescriptionController : Controller
    {
        private IPrescriptionRepository repository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PrescriptionController(IPrescriptionRepository repo)
        {
            repository = repo;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Prescriptions);

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int prescriptionId) => View(repository.Prescriptions.FirstOrDefault(p => p.PrescriptionId == prescriptionId));

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                prescription.PrescriptionDate = DateTime.Now;
                repository.SavePrescription(prescription);
                log.Info($"Диагноз {prescription.PrescriptionId} отредактирован или создан.");
                TempData["message"] = $"{prescription.PrescriptionId} был сохранен";
                return RedirectToAction("Index");
            }
            else
            {
                return View(prescription);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Create() => View("Edit", new Prescription());
    }
}