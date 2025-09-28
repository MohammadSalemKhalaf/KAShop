using KAShop.Model;
using Microsoft.EntityFrameworkCore;
namespace API_Task1.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-8JS28SG;Database=ASP-11;TrustServerCertificate=True;Trusted_Connection=True");
        }



    }
}
