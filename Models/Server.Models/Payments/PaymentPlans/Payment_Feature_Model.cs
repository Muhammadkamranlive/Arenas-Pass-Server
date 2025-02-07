using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class Payment_Feature_Model
    {
        public int Id                    { get; set; }
        public string FeatureTitle       { get; set; }
        public string FeatureDescription { get; set; }
        public int PaymentPlanId         { get; set; }
    }
}
