using KAShop.Model.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace API_Task1.Data
{
    public class ApplicationDbContext : DbContext
    {
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }



    }
}
