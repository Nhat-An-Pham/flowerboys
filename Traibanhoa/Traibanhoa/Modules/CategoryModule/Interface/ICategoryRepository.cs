using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Repository.Utils.Repository.Interface;
using Models.Models;

namespace Traibanhoa.Modules.CategoryModule.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<ICollection<Category>> GetCategoriesBy(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, ICollection<Category>> options = null,
            string includeProperties = null
        );
    }
}
