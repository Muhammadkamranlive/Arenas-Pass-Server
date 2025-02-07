using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Log_Service:Base_Service<AdminLogs>,ILog_Service
    {
        public Log_Service(IUnit_Of_Work_Repo unitOfWork,ILog_Repo log_Repo):base(unitOfWork,log_Repo)
        {

        }
    }
}
