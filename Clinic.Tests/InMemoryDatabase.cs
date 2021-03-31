using Clinic.Database;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Tests
{
    internal static class InMemoryDatabase
    {
        public static DbContextOptions<ApplicationDbContext> GetOptions(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return options;
        }
    }
}
