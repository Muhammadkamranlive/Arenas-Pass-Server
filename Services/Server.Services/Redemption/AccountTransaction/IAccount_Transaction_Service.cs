using System;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IAccount_Transaction_Service:IBase_Service<Account_Transaction>
    {
        Task<ResponseModel<string>> RedeemGiftCard(Redeem_Gift_Card_Model redeem_Gift);
        Task<ResponseModel<string>> RedeemGiftCardByUserId(Redeem_Gift_CardByUserId_Model model);
    }
}
