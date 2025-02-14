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
    public interface IPayment_Features_Service:IBase_Service<PaymentFeatureList>
    {
        Task<ResponseModel<string>> AddFeatures(Payment_Feature_Model model);
        Task<ResponseModel<string>> UpdateFeatures(Payment_Feature_Model model)   ;
    }
}
