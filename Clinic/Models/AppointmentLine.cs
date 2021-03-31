using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public class AppointmentLine
    {
        public int AppointmentLineId { get; set; }
        public int AppointmentId { get; set; }
        public int ServiceId { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal Price { get; set; }

        public Service Service { get; set; }
        public Appointment Appointment { get; set; }
    }
}