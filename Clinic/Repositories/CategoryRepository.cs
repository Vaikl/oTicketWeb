using Clinic.Database;
using Clinic.Interfaces;
using Clinic.Models;
using System.Collections.Generic;

namespace Clinic.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Category> Categories => _applicationDbContext.Categories;
    }
}