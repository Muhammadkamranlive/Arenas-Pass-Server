﻿using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.TaskManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskManagerController : ParentController<GENERALTASK, GeneralTaskModel>
    {
        private readonly IGeneralTask_Service _Service;
        private readonly IAuth_Manager_Service         _authManager;
        public TaskManagerController
        (
            IGeneralTask_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
