﻿using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Profile_Service:Base_Service<Personal>,IProfile_Service
    {
        public Profile_Service(IUnit_Of_Work_Repo unitOfWork, IPersonal_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
