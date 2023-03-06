using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models.Models;
using Repository.Utils.Repository.Interface;

namespace Traibanhoa.Modules.SubCateModule.Interface
{
    public interface ISubCateRepository : IRepository<SubCategory>
    {
        public Task<ICollection<SubCategory>> GetSubCatesBy(
            Expression<Func<SubCategory, bool>> filter = null,
            Func<IQueryable<SubCategory>, ICollection<SubCategory>> options = null,
            string includeProperties = null
        );
    }
}

