using Microsoft.EntityFrameworkCore;
using Cema.Models;

namespace Cema.Data
{
    public class CemaDbContext : DbContext
    {
        public CemaDbContext(DbContextOptions<CemaDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseGroup> ExpenseGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This is often configured in Startup.cs or Program.cs in a real application
                // using AddDbContext. For demonstration purposes, it's here.
                // Make sure "DefaultConnection" is defined in your appsettings.json
                // or another configuration source.
                optionsBuilder.UseSqlite("Data Source=cema.db"); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships if needed
            modelBuilder.Entity<Expense>()
                .HasOne<Car>()
                .WithMany()
                .HasForeignKey(e => e.CarId);

            // Seed initial data if necessary (optional)
            // modelBuilder.Entity<ExpenseGroup>().HasData(
            //     new ExpenseGroup { Id = 1, Name = "Gas" },
            //     new ExpenseGroup { Id = 2, Name = "Mechanical" },
            //     new ExpenseGroup { Id = 3, Name = "Insurance" },
            //     new ExpenseGroup { Id = 4, Name = "Cleaning" }
            // );
        }
    }
}