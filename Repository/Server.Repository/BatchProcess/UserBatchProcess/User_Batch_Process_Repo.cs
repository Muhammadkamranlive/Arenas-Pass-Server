using System;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Server.Repository
{
    public class User_Batch_Process_Repo : Repo<UserVoucher>, IUser_Batch_Process_Repo
    {
        public User_Batch_Process_Repo
        (
            ERPDb coursecontext, IHttpContextAccessor httpContextAccessor
        ) : base(coursecontext, httpContextAccessor)
        {
        }
    }
}
