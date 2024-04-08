using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class Dependent
    {
        public Guid Id              { get; set; }
        public string userId        { get; set; }
        public string Name          { get; set; }
        public string Relationship  { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasSpecialNeeds { get; set; }
        public string Notes         { get; set; }

        public int TenantId { get; set; } = 1;
    }
}
