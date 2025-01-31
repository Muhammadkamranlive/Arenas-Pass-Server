using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class PaymentFeatureList
    {
        public int Id                    { get; set; }
        public string FeatureTitle       { get; set; }
        public string FeatureDescription { get; set; }
        public int PaymentPlanId         { get; set; }

    }
}
