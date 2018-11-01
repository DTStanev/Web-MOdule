using ChushkaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChushkaApp.Data
{
    public class ChushkaDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Chushka;Integrated Security=True;");
        }
    }
}
