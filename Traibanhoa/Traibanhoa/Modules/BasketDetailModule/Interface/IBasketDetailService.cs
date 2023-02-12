using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Traibanhoa.Modules.TypeModule.Request;
using Traibanhoa.Modules.TypeModule.Response;
using Models.Models;
using Traibanhoa.Modules.BasketDetailModule.Request;

namespace Traibanhoa.Modules.BasketDetailModule.Interface
{
    public interface IBasketDetailService
    {
        public Task<ICollection<BasketDetail>> GetBasketDetailsBy(
            Expression<Func<BasketDetail, bool>> filter = null,
            Func<IQueryable<BasketDetail>, ICollection<BasketDetail>> options = null,
            string includeProperties = null);

        public Task<Boolean> AddNewBasketDetail(CreateBasketDetailRequest BasketDetailCreate);

        public Task<Boolean> UpdateBasketDetail(UpdateBasketDetailRequest BasketDetailUpdate);

        //public Task<Boolean> DeleteBasketDetail(BasketDetail BasketDetailDelete);

        public Task<ICollection<BasketDetail>> GetAll();

        public Task<BasketDetail> GetBasketDetailByID(Guid? id);
    }
}
