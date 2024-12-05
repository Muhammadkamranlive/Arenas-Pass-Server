using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Domain
{
    public class UserVoucher
    {
        public int    Id                    { get; set; }
        public int    TenantId              { get; set; }
        public string Status                { get; set; }
        public string Email                 { get; set; }
        public string FirstName             { get; set; }
        public string LastName              { get; set; }
        public string Remarks               { get; set; }
        public string BatchNo               { get; set; }
        public string UploadedByUserId      { get; set; }
        public string UploadedByUsername    { get; set; }
        public string? ProcessedByUserId    { get; set; }
        public string? ProcessedByUsername  { get; set; }
        public DateTime CreatedAt           { get; set; }
        public string  UserType             { get; set; }
        public UserVoucher()
        {
            CreatedAt= DateTime.Now;
        }

    }
}
