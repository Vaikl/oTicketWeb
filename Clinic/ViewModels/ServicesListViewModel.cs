using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.ViewModels
{
    public class ServicesListViewModel
    {
        public IEnumerable<Service> Services { get; set; }
        public string CurrentCategory { get; set; }
    }
}