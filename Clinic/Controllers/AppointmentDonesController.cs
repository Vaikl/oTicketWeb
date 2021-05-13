using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Clinic.Database;
using Clinic.Models;

namespace Clinic.Controllers
{
    public class AppointmentDonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentDonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppointmentDones
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppointmentsDone.ToListAsync());
        }

        // GET: AppointmentDones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentDone = await _context.AppointmentsDone
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointmentDone == null)
            {
                return NotFound();
            }

            return View(appointmentDone);
        }

        // GET: AppointmentDones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppointmentDones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,PatientId,PatientFullName,DoctorId,DiagnosisId,DiagnosName,TotalSum,AppointmentPlaced")] AppointmentDone appointmentDone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentDone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentDone);
        }

        // GET: AppointmentDones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentDone = await _context.AppointmentsDone.FindAsync(id);
            if (appointmentDone == null)
            {
                return NotFound();
            }
            return View(appointmentDone);
        }

        // POST: AppointmentDones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentId,PatientId,PatientFullName,DoctorId,DiagnosisId,DiagnosName,TotalSum,AppointmentPlaced")] AppointmentDone appointmentDone)
        {
            if (id != appointmentDone.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentDone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentDoneExists(appointmentDone.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentDone);
        }

        // GET: AppointmentDones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentDone = await _context.AppointmentsDone
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointmentDone == null)
            {
                return NotFound();
            }

            return View(appointmentDone);
        }

        // POST: AppointmentDones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentDone = await _context.AppointmentsDone.FindAsync(id);
            _context.AppointmentsDone.Remove(appointmentDone);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentDoneExists(int id)
        {
            return _context.AppointmentsDone.Any(e => e.AppointmentId == id);
        }
    }
}
