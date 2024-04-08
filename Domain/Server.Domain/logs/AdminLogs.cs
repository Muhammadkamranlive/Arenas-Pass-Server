using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class AdminLogs
    {
        public Guid Id              { get; set; }
        public string OperationType { get; set; } = "Add";
        public string EntityType    { get; set; }
        public string Content       { get; set; }
        public DateTime Timestamp   { get; set; }
        public string UserId        { get; set; }="";
        public int    TenantId      { get; set; }=1;
    }
}
