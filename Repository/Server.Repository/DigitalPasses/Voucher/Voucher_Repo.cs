using Server.Core;
using Server.Domain;
using Microsoft.AspNetCore.Http;

namespace Server.Repository
{
    public class Voucher_Repo : Repo<Voucher>, IVoucher_Repo
    {
        public Voucher_Repo
        (
            ERPDb coursecontext, 
            IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
