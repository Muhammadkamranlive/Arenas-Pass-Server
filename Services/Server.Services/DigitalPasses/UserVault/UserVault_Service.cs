using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class UserVault_Service : Base_Service<UsersVault>, IUserVault_Service
    {
        public UserVault_Service(IUnitOfWork unitOfWork, IUser_Vault_Repo genericRepository) : base(unitOfWork, genericRepository)
        {
        }
    }
}
