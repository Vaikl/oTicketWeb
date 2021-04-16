using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Clinic.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ShoppingCart _shoppingCart;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;

        public AppointmentRepository(
            ApplicationDbContext applicationDbContext,
            ShoppingCart shoppingCart,
            UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor)
        {
            this.applicationDbContext = applicationDbContext;
            _shoppingCart = shoppingCart;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public IEnumerable<Appointment> Appointments => applicationDbContext.Appointments.ToList();

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return Appointments.OrderBy(p => p.AppointmentId).ToList();
        }

        public void CreateAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                var shoppingCartItems = _shoppingCart.ShoppingCartItems;

                decimal appointmentTotalSum = 0;
                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    appointmentTotalSum += shoppingCartItem.Amount * shoppingCartItem.Service.Price;
                }

                appointment.AppointmentPlaced = DateTime.Now;
                appointment.TotalSum = appointmentTotalSum;
                appointment.PatientId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                appointment.DiagnosisId = 3;
                ApplicationUser user = _userManager.Users.Where(x => x.Id == appointment.PatientId).First();
                appointment.PatientFullName = user.FirstName + " " + user.LastName;
                appointment.DiagnosName = applicationDbContext.Diagnoses.Where(x => x.DiagnosisId == appointment.DiagnosisId).First().Name;
                applicationDbContext.Appointments.Add(appointment);
                applicationDbContext.SaveChanges();

                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    var appointmentLine = new AppointmentLine()
                    {
                        Amount = shoppingCartItem.Amount,
                        ServiceId = shoppingCartItem.Service.ServiceId,
                        AppointmentId = appointment.AppointmentId,
                        Price = shoppingCartItem.Service.Price
                    };

                    applicationDbContext.AppointmentLines.Add(appointmentLine);
                }

                applicationDbContext.SaveChanges();
            }
        }

        public void SaveAppointment(Appointment appointment)
        {
                Appointment dbEntry = applicationDbContext.Appointments.FirstOrDefault(d => d.AppointmentId == appointment.AppointmentId);

                if (appointment != null && dbEntry != null)
                {
                    dbEntry.Diagnosis = appointment.Diagnosis;
                }
         
            applicationDbContext.SaveChanges();
        }

        public Appointment DeleteAppointment(int appointmentId)
        {
            Appointment dbEntry = applicationDbContext.Appointments.FirstOrDefault(d => d.AppointmentId == appointmentId);

         
            if (dbEntry != null)
            {
                applicationDbContext.Appointments.Remove(dbEntry);
                applicationDbContext.SaveChanges();
            }

            return dbEntry;
        }
    }
}