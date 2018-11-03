using MeTube.Models;
using Microsoft.EntityFrameworkCore;

namespace MeTube.Data
{
    public class MeTubeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public DbSet<Tube> Tubes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=MeTube;Integrated Security=True;");
        }
    }
}
