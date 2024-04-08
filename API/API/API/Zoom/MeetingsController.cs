using AutoMapper;
using Server.Domain;
using Server.Models;
using API.Controllers;
using Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Zoom
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ParentController<ZoomMeetings, ZooMeetingModel>
    {
        private readonly IZoomService _zoomService;
        private readonly IZoomMeeting_Service _zoomMeetingService;
        private readonly IAuthManager _authManager;
        private readonly IEmail_Service _emailService;
        public MeetingsController
        (
          IAuthManager authManager,
          IZoomMeeting_Service service,
          IMapper mapper,
          IZoomService zoomService,
          IEmail_Service emailService


        ) : base(service, mapper)
        {
            _zoomService = zoomService;
            _zoomMeetingService = service;
            _authManager = authManager;
            _emailService = emailService;
        }


        [HttpPost]
        [Route("AddMeetings")]
        [CustomAuthorize("Write")]
        public async Task<IActionResult> AddMeetings(MeetingAdd meeting)
        {
            try
            {
                var accessToken = await _zoomService.GetAccessTokenAsync();

                dynamic Response = await _zoomService.CreateZoomMeetingAsync(accessToken, meeting);
                if (Response is not string)
                {
                    MeetingResponse meetingResponse = Response;
                    ZoomMeetings zoomMeting = new ZoomMeetings();
                    zoomMeting.email = meeting.Email;
                    zoomMeting.Date = meetingResponse.start_time;
                    zoomMeting.start_time = meetingResponse.start_time;
                    zoomMeting.MeetingNumber = Convert.ToString(meetingResponse.id);
                    zoomMeting.Topic = meeting.Topic;
                    zoomMeting.password = meetingResponse.password;
                    zoomMeting.description = meeting.description;
                    zoomMeting.meetingstatus = "Scheduled";
                    zoomMeting.join_url = meetingResponse.join_url;
                    zoomMeting.start_url = meetingResponse.start_url;
                    zoomMeting.created_at = meetingResponse.created_at;
                    zoomMeting.timezone = meeting.meetingType; ;
                    zoomMeting.meetingBanner = meeting.meetingBanner;
                    zoomMeting.senderId = meeting.senderId;
                    zoomMeting.recieverId = meeting.recieverId;
                    await _zoomMeetingService.InsertAsync(zoomMeting);
                    await _zoomMeetingService.CompleteAync();
                    string subject = "Zoom Meeting For " + meeting.Topic;
                    string salutation = "Dear Admin";
                    string messageBody = $"<h4>You have created meeting with an email <br/> {meeting.Email} <br/> Topic {meeting.Topic} </h4>";
                    string emailMessage = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmailAsync(meeting.Email, subject, emailMessage);
                    var message = "Zoom Meeting Added" + typeof(ZoomMeetings)?.Name;
                    return Content($"{{ \"message\": \"{message}\" }}", "application/json");
                }
                return Ok(Response);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }


        [HttpGet]
        [Route("SenderMeeting")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> SenderMeeting(string uid)
        {
            try
            {

                DateTime currentUtcTime = DateTime.UtcNow;
                var meeting = await _zoomMeetingService.Find(x => (x.senderId == uid || x.recieverId == uid) && x.start_time >= currentUtcTime);
                return Ok(meeting);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }



        [HttpGet]
        [Route("AdminMeeting")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> AdminMeeting()
        {
            try
            {

                DateTime currentUtcTime = DateTime.UtcNow;
                var meeting = await _zoomMeetingService.Find(x => x.start_time >= currentUtcTime);
                return Ok(meeting);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }

        }


    }
}
