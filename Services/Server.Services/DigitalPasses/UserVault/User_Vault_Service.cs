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
    public class User_Vault_Service : Base_Service<Vault>, IUser_Vault_Service
    {
        public User_Vault_Service(IUnit_Of_Work_Repo unitOfWork, IUser_Vault_Repo genericRepository) : base(unitOfWork, genericRepository)
        {
        }
    }
}
