using System;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public interface IBulk_User_Import_Service
    {
        Task<ResponseModel<string>> BulkInsertUsersAsync(IList<ApplicationUser> users, string Role);
    }
}
