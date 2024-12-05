using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public interface IValidate_Txn_Service
    {
        Task<ResponseModel<string>> ValidateTxn(Account_Transaction account_Transaction,decimal PrevtxnAmount);
    }
}
