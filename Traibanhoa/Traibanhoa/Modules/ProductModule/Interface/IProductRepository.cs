using Repository.Utils.Repository.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;

namespace Traibanhoa.Modules.ProductModule.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<ICollection<Product>> GetProductsBy(
               Expression<Func<Product, bool>> filter = null,
               Func<IQueryable<Product>, ICollection<Product>> options = null,
               string includeProperties = null
           );
    }
}
