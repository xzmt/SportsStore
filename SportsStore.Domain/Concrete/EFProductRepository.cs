using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entitiess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }
}
