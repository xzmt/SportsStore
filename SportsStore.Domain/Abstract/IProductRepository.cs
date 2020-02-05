using SportsStore.Domain.Entitiess;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStore.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
    }
}

