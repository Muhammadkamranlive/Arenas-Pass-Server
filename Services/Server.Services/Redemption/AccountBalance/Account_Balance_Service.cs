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
    public class Account_Balance_Service : Base_Service<Account_Balance>, IAccount_Balance_Service
    {
        public Account_Balance_Service
        (
            IUnit_Of_Work_Repo unitOfWork, 
            IAccount_Balance_Repo genericRepository
        ) : base(unitOfWork, genericRepository)
        {
        }
    }
}
