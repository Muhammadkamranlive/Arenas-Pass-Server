using System;
using System.Linq;
using System.Text;
using Server.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Domain.DigitalPass.Transaction;

namespace Server.Services
{
    public interface ITransaction_No_Service:IBase_Service<Transaction_No>
    {
        Task<int> GetTxnNo();
    }
}
