using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Repository;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class User_Batch_Process_Service:Base_Service<UserVoucher>, IUser_Batch_Process_Service
    {
        private readonly IUser_Batch_Process_Repo iRepo;
        public User_Batch_Process_Service
        (
           IUnitOfWork unitOfWork,
           IUser_Batch_Process_Repo  Repo
        ):base(unitOfWork, Repo)
        {
            iRepo=Repo;
        }
    }
}
