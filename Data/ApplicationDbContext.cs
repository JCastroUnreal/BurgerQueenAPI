using Microsoft.EntityFrameworkCore;
using BurgerQueenAPI.Models;

namespace BurgerQueenAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Representa la tabla de Products en la base de datos
        /// </summary>
        public DbSet<Product> Products { get; set; }
    }
}
