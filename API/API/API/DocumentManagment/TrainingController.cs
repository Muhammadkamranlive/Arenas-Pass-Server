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
    public class TrainingController : ParentController<Trainings, TrainingModel>
    {
        private readonly ITraining_Service _Service;
        private readonly IAuthManager _authManager;
        public TrainingController
        (
            ITraining_Service service,
            IMapper mapper,
            IAuthManager authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
