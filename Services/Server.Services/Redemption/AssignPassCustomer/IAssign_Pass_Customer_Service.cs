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
    public interface IAssign_Pass_Customer_Service:IBase_Service<Account_Transaction>
    {
        Task<ResponseModel<string>> AssignGiftPassToCustomer(Assign_Pass_Model model);
       
    }
}
