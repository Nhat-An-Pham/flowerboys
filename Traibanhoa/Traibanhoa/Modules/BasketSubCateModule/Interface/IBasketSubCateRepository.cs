﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Repository.Utils.Repository.Interface;
using Models.Models;

namespace Traibanhoa.Modules.BasketSubCateModule.Interface
{
    public interface IBasketSubCateRepository : IRepository<BasketSubCate>
    {
        public Task<ICollection<BasketSubCate>> GetBasketSubCatesBy(
            Expression<Func<BasketSubCate, bool>> filter = null,
            Func<IQueryable<BasketSubCate>, ICollection<BasketSubCate>> options = null,
            string includeProperties = null
        );
    }
}
