using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.RequestBasketDetailModule.Request;

namespace Traibanhoa.Modules.RequestBasketDetailModule.Interface
{
    public interface IRequestBasketDetailService
    {
        public Task<ICollection<RequestBasketDetail>> GetRequestBasketDetailsBy(
            Expression<Func<RequestBasketDetail, bool>> filter = null,
            Func<IQueryable<RequestBasketDetail>, ICollection<RequestBasketDetail>> options = null,
            string includeProperties = null);

        public Task<Guid?> AddNewRequestBasketDetail(CreateRequestBasketDetailRequest RequestBasketDetailCreate);

        public Task UpdateRequestBasketDetail(UpdateRequestBasketDetailRequest RequestBasketDetailUpdate);

        public Task DeleteRequestBasketDetail(Guid? requestBasketDetailDeleteID);

        public Task<ICollection<RequestBasketDetail>> GetAll();

        public Task<RequestBasketDetail> GetRequestBasketDetailByID(Guid? id);
    }
}
