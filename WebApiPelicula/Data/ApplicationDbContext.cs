using Microsoft.EntityFrameworkCore;
using WebApiPelicula.Models;

namespace WebApiPelicula.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }
    }
}
