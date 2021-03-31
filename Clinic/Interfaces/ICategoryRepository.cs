using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}