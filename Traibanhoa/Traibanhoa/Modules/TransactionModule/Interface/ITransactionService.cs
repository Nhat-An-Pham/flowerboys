using Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Traibanhoa.Modules.TransactionModule.Interface
{
    public interface ITransactionService
    {
        public Task<ICollection<Transaction>> GetAll();
    }
}
