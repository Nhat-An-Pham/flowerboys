using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Utils.Repository;
using Traibanhoa.Modules.SubCateModule.Interface;

namespace Traibanhoa.Modules.SubCateModule
{
    public class SubCateRepository : Repository<SubCategory>, ISubCateRepository
    {
        private readonly TraibanhoaContext _db;

        public SubCateRepository(TraibanhoaContext db) : base(db)
        {
            _db = db;
        }
        public async Task<ICollection<SubCategory>> GetSubCatesBy(
            Expression<Func<SubCategory, bool>> filter = null,
            Func<IQueryable<SubCategory>, ICollection<SubCategory>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<SubCategory> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}

