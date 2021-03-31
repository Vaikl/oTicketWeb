using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Clinic.Controllers
{
    public class DiagnosisController : Controller
    {
        private IDiagnosisRepository repository;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DiagnosisController(IDiagnosisRepository repo)
        {
            repository = repo;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Index() => View(repository.Diagnoses);

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Edit(int diagnosisId) => View(repository.Diagnoses.FirstOrDefault(p => p.DiagnosisId == diagnosisId));

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Edit(Diagnosis diagnosis)
        {
            if (ModelState.IsValid)
            {
                repository.SaveDiagnosis(diagnosis);
                log.Info($"Диагноз {diagnosis.Name} отредактирован или создан.");
                TempData["message"] = $"{diagnosis.Name} был сохранен";
                return RedirectToAction("Index");
            }
            else
            {
                return View(diagnosis);
            }
        }

        [Authorize(Roles = "Admin, Doctor")]
        public ViewResult Create() => View("Edit", new Diagnosis());

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost]
        public IActionResult Delete(int diagnosisId)
        {
            Diagnosis deletedDiagnosis = repository.DeleteDiagnosis(diagnosisId);
            log.Info($"Диагноз {deletedDiagnosis} удален.");
            if (deletedDiagnosis != null)
            {
                TempData["message"] = $"{deletedDiagnosis.Name} был удален";
            }
            return RedirectToAction("Index");
        }
    }
}