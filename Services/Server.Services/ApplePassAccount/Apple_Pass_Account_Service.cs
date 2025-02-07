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
    public class Apple_Pass_Account_Service : Base_Service<Apple_Pass_Account>, IApple_Pass_Account_Service
    {
        public Apple_Pass_Account_Service
        (
            IUnit_Of_Work_Repo unitOfWork, IApple_Pass_Account_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
        }


        
    }
}
