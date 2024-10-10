using CRUD.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\CRUD.Data\Data\UsersData.db"));
            Console.WriteLine($"Database path: {dbPath}");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
