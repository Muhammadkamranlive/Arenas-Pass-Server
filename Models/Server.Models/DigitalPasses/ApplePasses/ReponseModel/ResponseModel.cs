using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Models
{
    public class ResponseModel<T> where T : class
    {
        public string Status_Code { get; set; }
        public string Description { get; set; }
        public object Response    { get; set; }
    }
}
