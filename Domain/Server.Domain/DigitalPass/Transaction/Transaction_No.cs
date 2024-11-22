using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain.DigitalPass.Transaction
{
    public class Transaction_No
    {
        public int      Id        { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EntityType  { get; set; }
        public Transaction_No()
        {
            CreatedAt = DateTime.UtcNow;  
        }
    }
}
