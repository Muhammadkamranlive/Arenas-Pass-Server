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
    public interface IArenas_Payment_Plans_Service:IBase_Service<Payment_Plans>
    {
        Task<ResponseModel<string>> Add(PaymentPlansModel model);
        Task<ResponseModel<string>> UpdatePaymentPlan(PaymentPlansModel model);
    }
}
