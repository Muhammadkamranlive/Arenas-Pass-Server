﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class TenantKeyHistory
    {
        public int    Id          { get; set; }
        public string privateKey  { get; set; }
        public string publicKey   { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TenantId       { get; set; }
        public string Change_By   { get; set; }
        public TenantKeyHistory()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
