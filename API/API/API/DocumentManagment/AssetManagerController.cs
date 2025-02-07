using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.DocumentManagment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetManagerController : ParentController<Asset, AssetModel>
    {
        private readonly IAssetManager_Service   _Service;
        private readonly IAuth_Manager_Service            _authManager;
        public AssetManagerController
        (
            IAssetManager_Service service,
            IMapper mapper,
            IAuth_Manager_Service  authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
