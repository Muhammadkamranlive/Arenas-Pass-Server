using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.CustomerCare
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ParentController<Case, CaseModel>
    {
        private readonly ICaseManagment_Service _Service;
        private readonly IAuthManager           _authManager;
        public CaseController
        (
            ICaseManagment_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
