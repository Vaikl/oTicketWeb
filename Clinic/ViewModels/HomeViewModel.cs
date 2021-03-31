using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Service> PrefferedServices { get; set; }
    }
}