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
    public class JobExperienceController : ParentController<JobExperience, JobExperienceModel>
    {
        private readonly IJobExperience_Service    _Service;
        private readonly IAuthManager             _authManager;
        public JobExperienceController
        (
            IJobExperience_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
