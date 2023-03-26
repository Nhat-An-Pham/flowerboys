using Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Traibanhoa.Modules.TransactionModule.Interface;

namespace Traibanhoa.Modules.TransactionModule
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _TransactionRepository;

        public TransactionService(ITransactionRepository TransactionRepository)
        {
            _TransactionRepository = TransactionRepository;
        }

        public async Task<ICollection<Transaction>> GetAll()
        {
            return await _TransactionRepository.GetAll();
        }
    }
}