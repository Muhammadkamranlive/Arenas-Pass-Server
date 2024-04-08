
namespace Server.Services
{
    public interface IEmail_Service
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendBulkEmailAsync(List<string> toList, string subject, string content, bool isHtml = true);
    }
}
