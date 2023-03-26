using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models.Models;
using Repository.Utils.Repository.Interface;

namespace Traibanhoa.Modules.TransactionModule.Interface
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        public Task<ICollection<Transaction>> GetTransactionsBy(
               Expression<Func<Transaction, bool>> filter = null,
               Func<IQueryable<Transaction>, ICollection<Transaction>> options = null,
               string includeProperties = null
           );
    }
}
