using System.Text;
using Server.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Services
{
    public class ZoomService : IZoomService
    {
        private readonly HttpClient _httpClient;
        public ZoomService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var clientId     = "r8Lafx5WQYWhJtUi6PEP0Q";
                var clientSecret = "ND4AUKBHkCTho6OsYfDPloYZ4wsLA2AT";
                var accountId    = "8mjnXAWGSD6mZ3bG93Tpmw";

                var tokenUrl = "https://zoom.us/oauth/token";
                var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));

                var requestContent = new FormUrlEncodedContent(new[]
                {
                  new KeyValuePair<string, string>("grant_type", "account_credentials"),
                  new KeyValuePair<string, string>("account_id", accountId)
                });

                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Credentials}");

                var response = await _httpClient.PostAsync(tokenUrl, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into the ZoomTokenResponse object
                    var tokenResponse = JsonConvert.DeserializeObject<ZoomTokenResponse>(responseContent);

                    return tokenResponse.access_token;
                }
                else
                {
                    // Handle error cases here
                    throw new Exception("Failed to retrieve access token.");
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
        public async Task<dynamic> CreateZoomMeetingAsync(string accessToken, MeetingAdd meeting)
        {
            try
            {
                var createMeetingUrl = "https://api.zoom.us/v2/users/me/meetings";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var meetingInfo = new
                {
                    topic = meeting.Topic,
                    type = 2, // Scheduled Meeting
                    start_time = meeting.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    duration = 40,
                    password = GenerateRandomPassword(),
                    settings = new
                    {
                        host_video = true,
                        participant_video = false,
                        recording = "host",
                    },
                    schedule_for = "Muhammadkamranntu@gmail.com"
                };

                var json = JsonConvert.SerializeObject(meetingInfo);

                // Create a request content with JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make a POST request to create the meeting
                var response = await _httpClient.PostAsync(createMeetingUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var meetingResponse = JsonConvert.DeserializeObject<MeetingResponse>(responseContent);
                    return meetingResponse;
                }
                else
                {
                    // Handle error cases here
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> GenerateZoomToken(ZoomTokenRequest request)
        {
            try
            {
                string ZOOM_MEETING_SDK_KEY = "r8Lafx5WQYWhJtUi6PEP0Q";
                string ZOOM_MEETING_SDK_SECRET = "ND4AUKBHkCTho6OsYfDPloYZ4wsLA2AT";

                long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 30;
                long exp = iat + 60 * 60 * 2;

                var payload = new
                {
                    sdkKey = ZOOM_MEETING_SDK_KEY,
                    mn = request.MeetingNumber,
                    role = request.Role,
                    iat = iat,
                    exp = exp,
                    appKey = ZOOM_MEETING_SDK_KEY,
                    tokenExp = iat + 60 * 60 * 2
                };

                string signature = GenerateSignature(ZOOM_MEETING_SDK_SECRET, payload);

                return signature;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
        private static string GenerateSignature(string secret, object payload)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secret);

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a ClaimsIdentity and add the custom claim
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("payload", JsonConvert.SerializeObject(payload)));

            var token = new JwtSecurityToken(
                issuer: "your_issuer", // Replace with your issuer
                audience: "your_audience", // Replace with your audience
                claims: claimsIdentity.Claims,
                expires: DateTime.UtcNow.AddHours(2), // Adjust expiration as needed
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        private static string GenerateRandomPassword()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new char[6];

            for (int i = 0; i < 6; i++)
            {
                password[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(password);
        }

    }
}
