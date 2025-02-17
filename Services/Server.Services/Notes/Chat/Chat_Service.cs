﻿using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Chat_Service:Base_Service<Chat>, IChat_Service
    {
        public Chat_Service(IUnit_Of_Work_Repo unitOfWork, IChat_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
