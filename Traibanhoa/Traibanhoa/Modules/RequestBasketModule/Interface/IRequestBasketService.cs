using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.RequestBasketModule.Request;
using Traibanhoa.Modules.BasketModule.Request;
using Traibanhoa.Modules.BasketModule.Response;
using Traibanhoa.Modules.RequestBasketModule.Response;

namespace Traibanhoa.Modules.RequestBasketModule.Interface
{
    public interface IRequestBasketService
    {
        public Task<ICollection<RequestBasket>> GetRequestBasketsBy(
            Expression<Func<RequestBasket, bool>> filter = null,
            Func<IQueryable<RequestBasket>, ICollection<RequestBasket>> options = null,
            string includeProperties = null);

        public Task<Guid> AddNewRequestBasket(Guid? currentCustomerId);

        public Task UpdateRequestBasket(UpdateRequestBasketRequest requestBasketUpdate);

        public Task DeleteRequestBasket(Guid? requestBasketDeleteID);

        public Task<ICollection<GetRequestBasketResponse>> GetAll();
        public Task<RequestBasket> GetRequestBasketByID(Guid? id);
        public Task<RequestBasket> GetRequestBasketByAuthorID(Guid? id);
        public Task<RequestBasket> GetRequestBasketByConfirmerID(Guid? id);
    }
}
