using Server.Models;
using Server.Domain;
using Server.Services;
using System.Collections;
using Microsoft.AspNetCore.SignalR;

namespace API.RealTime
{
    public class ChatHub : Hub
    {
        private readonly INotifications_Service _notifications_Service;
        private readonly ICase_Comments_Service _caseComments_Service;
        private readonly ICase_Managment_Service caseManageService;
        private readonly IChat_Service _chat_Service;
        private IAuth_Manager_Service _authManager;
        public ChatHub
        (
            INotifications_Service notifications_Service,
            ICase_Comments_Service caseComments_Service,
            ICase_Managment_Service service,
            IAuth_Manager_Service authManager,
            IChat_Service chat_Service
        )
        {
            _notifications_Service = notifications_Service;
            _caseComments_Service = caseComments_Service;
            caseManageService = service;
            _authManager = authManager;
            _chat_Service = chat_Service;
        }

        public async Task SendNotification(string userId,Guid caseId, string message)
        {
            AllUsersModel user =await _authManager.GetByIduser(userId);
            // Save the new notification to the database
            var newNotification = new CaseComment
            {
                Id        = Guid.NewGuid(),
                CaseId    = caseId,
                UserId    = userId,
                CreatedAt = DateTime.UtcNow,
                Text      = message
              
            };

            await _caseComments_Service.InsertAsync(newNotification);
            await _caseComments_Service.CompleteAync();

            var allNotifications = new ChatModel1
            {
                CreatedAt= newNotification.CreatedAt,
                Text=newNotification.Text,
                Email=user.Email,
                Id=newNotification.Id,
                Image=user.Image,
                Name=user.FirstName+" "+user.LastName,
                Roles =user.Roles
            };
            await Clients.All.SendAsync("ReceiveNotification", allNotifications);
        }


        private  async Task<IList> GetCaseComments(Guid id)
        {

            try
            {
                var comments = await _caseComments_Service.GetAll();
                IList<AllUsersModel> users = (IList<AllUsersModel>)await _authManager.GetAll();
                

                var list = comments.Join
                    (
                      users,
                      (co) => new { co.UserId },
                      (u) => new { UserId = u.Id },
                      (co, u) => new { co, u }
                    )
                   .Where(x=>x.co.CaseId== id)
                    .Select(x => new 
                    {
                        x.co.Id,
                        x.co.Text,
                        x.co.CreatedAt,
                        name = x.u.FirstName + " " + x.u.LastName,
                        x.u.Email,
                        x.u.Roles,
                        x.u.Image,

                    })
                    .OrderBy(x => x.CreatedAt)
                    .ToList();

               
                
                return list;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }




        public async Task SendMessage(string SenderId, string RecieverId, string message, string Role)
        {
            var item = new Chat()
            {

                Content    = message,
                ReceiverId = RecieverId,
                SenderId   = SenderId,
                Timestamp  = DateTime.Now,
                Role       = Role
            };
            await _chat_Service.InsertAsync(item);
            await _chat_Service.CompleteAync();

            var allNotifications = new Chat
            {
                ReceiverId = RecieverId,
                Content    = message,
                SenderId   = SenderId,
                Timestamp  = DateTime.Now,
                Role       = Role,
                Id         = Guid.NewGuid(),
            };
            await Clients.All.SendAsync("ReceiveMessage", allNotifications);
        }

    }
}
