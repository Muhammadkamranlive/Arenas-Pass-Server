using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class AssetManager_Service:Base_Service<Asset>, IAssetManager_Service
    {
        public AssetManager_Service(IUnitOfWork unitOfWork, IAssetManager_Repo _Repo) : base(unitOfWork, _Repo)
        {

        }
    }
}
