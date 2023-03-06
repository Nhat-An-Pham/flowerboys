using Models.Models;
using Repository.Utils.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.CartDetailModule.Interface
{
    public interface ICartDetailRepository : IRepository<CartDetail>
    {
        public Task<ICollection<CartDetail>> GetCartDetailsBy(
            Expression<Func<CartDetail, bool>> filter = null,
            Func<IQueryable<CartDetail>, ICollection<CartDetail>> options = null,
            string includeProperties = null
        );
    }
}
