using CRUDtasks.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDtasks.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Persons)
                .HasForeignKey(p => p.DepartmentId);
        }
    }

}
