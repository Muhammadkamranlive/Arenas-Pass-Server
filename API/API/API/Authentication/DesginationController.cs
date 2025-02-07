using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesginationController : ParentController<Designations, DesginationModel>
    {
        private readonly IDesignation_Service _Service;
        private readonly IAuth_Manager_Service _authManager;
        private readonly IMapper _mapper;
        public DesginationController
        (
            IDesignation_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _mapper = mapper;
            _authManager = authManager;
        }
    }
}
