using System;
using Server.UOW;
using System.Linq;
using System.Text;
using Server.Core;
using Server.Domain;
using Server.BaseService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Services
{
    public class ArenasBilling_Service : Base_Service<ArenasBilling>, IArenasBilling_Service
    {
        public ArenasBilling_Service
        ( IUnitOfWork unitOfWork, IRepo<ArenasBilling> genericRepository
        ) : base(unitOfWork, genericRepository)
        {
        }
    }
}
