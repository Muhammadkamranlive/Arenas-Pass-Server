using System;
using Server.UOW;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Domain.DigitalPass.Transaction;

namespace Server.Services.DigitalPasses.Transaction
{
    public class Transaction_No_Service : Base_Service<Transaction_No>, ITransaction_No_Service
    {
        private readonly ITransaction_No_Repo iRepo;
        public Transaction_No_Service
        (
            IUnitOfWork unitOfWork, 
            ITransaction_No_Repo genericRepository
        ) : base(unitOfWork, genericRepository)
        {
            iRepo = genericRepository;
        }

        public async Task<int> GetTxnNo()
        {
            Transaction_No ob = new()
            {
                EntityType = "ApplePasses"
            };
            Transaction_No txnObj= await iRepo.AddAsync(ob);
            return txnObj.Id;
        }
    }
}
