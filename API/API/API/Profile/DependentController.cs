using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Profile
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependentController : ParentController<Dependent, DependentModel>
    {
        private readonly IDependent_Service _Service;
        private readonly IAuthManager      _authManager;
        public DependentController
        (
            IDependent_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
