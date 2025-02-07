using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ParentController<NOTIFICATIONS, NotificationsModel>
    {
        private readonly INotifications_Service _Service;
        private readonly IAuth_Manager_Service           _authManager;
        public NotificationController
        (
            INotifications_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }
    }
}
