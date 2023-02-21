using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models.Models;
using Traibanhoa.Modules.RequestBasketModule.Request;

namespace Traibanhoa.Modules.RequestBasketModule.Interface
{
    public interface IRequestBasketService
    {
        public Task<ICollection<RequestBasket>> GetRequestBasketsBy(
            Expression<Func<RequestBasket, bool>> filter = null,
            Func<IQueryable<RequestBasket>, ICollection<RequestBasket>> options = null,
            string includeProperties = null);

        public Task<Guid?> AddNewRequestBasket(CreateRequestBasketRequest RequestBasketCreate);

        public Task UpdateRequestBasket(UpdateRequestBasketRequest RequestBasketUpdate);

        //public Task<Boolean> DeleteRequestBasket(RequestBasket RequestBasketDelete);

        public Task<ICollection<RequestBasket>> GetAll();

        public Task<RequestBasket> GetRequestBasketByID(Guid? id);
    }
}
