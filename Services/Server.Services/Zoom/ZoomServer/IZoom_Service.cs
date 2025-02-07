using Server.Models;

namespace Server.Services
{
    public interface IZoom_Service
    {
        Task<string> GetAccessTokenAsync();
        Task<dynamic> CreateZoomMeetingAsync(string accessToken, MeetingAdd meeting);
        Task<string> GenerateZoomToken(ZoomTokenRequest request);
    }
}
