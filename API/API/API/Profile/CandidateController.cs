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
    public class CandidateController : ParentController<CandidateInfo, CandidateModel>
    {
        private readonly ICandidate_Service _Service;
        private readonly IAuth_Manager_Service      _authManager;
        public CandidateController
        (
            ICandidate_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
