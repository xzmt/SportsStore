using SportsStore.Domain.Entitiess;
using System.Data.Entity;

namespace SportsStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
       public EFDbContext():base("name=SportsStore")
            {
            }
        public DbSet<Product> Products { get; set; }
    }
}
