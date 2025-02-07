using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.HRAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ParentController<Chat, ChatModel>
    {
        private readonly IChat_Service          _Service;
        private readonly IAuth_Manager_Service           _authManager;

        public ChatController
        (
            IChat_Service service,
            IMapper mapper,
            IAuth_Manager_Service authManager
        ) : base(service, mapper)
        {
            _Service = service;
            _authManager = authManager;
        }


        [HttpGet]
        [Route("GetChat")]
        public async Task<IActionResult> GetChat(string userId, string reciverId)
        {
            IList<Chat> chatMessages = await _Service.Find
                          (c =>
                               (c.SenderId == userId && c.ReceiverId == reciverId) ||
                               (c.SenderId == reciverId && c.ReceiverId == userId)
                          );

            return Ok(chatMessages.OrderBy(x => x.Timestamp));
        }

    }
}
