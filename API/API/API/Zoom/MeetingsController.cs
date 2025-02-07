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
    private readonly IZoom_Service _zoomService;
    private readonly IZoom_Meeting_Service _zoomMeetingService;
    private readonly IAuth_Manager_Service _authManager;
    private readonly IEmail_Service _emailService;
    public MeetingsController
    (
      IAuth_Manager_Service authManager,
      IZoom_Meeting_Service service,
      IMapper mapper,
      IZoom_Service zoomService,
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
          string emailMessage = CreatePasswordForgotEmailBody(meeting.Email, meeting.Topic, zoomMeting.start_time, zoomMeting.email, zoomMeting.start_url);
          await _emailService.SendEmail1Async(meeting.Email, subject, emailMessage);
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


    private static string CreatePasswordForgotEmailBody(string Username, string title, DateTime Date, string email, string url)
    {
      string emailTemplate = $@"
     <!doctype html>
        <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">

        <head>
        <title>

        </title>
        <!--[if !mso]><!-- -->
        <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
        <!--<![endif]-->
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
        <style type=""text/css"">
        #outlook a {{
            padding: 0;
        }}

        .ReadMsgBody {{
            width: 100%;
        }}

        .ExternalClass {{
            width: 100%;
        }}

        .ExternalClass * {{
            line-height: 100%;
        }}

        body {{
            margin: 0;
            padding: 0;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }}

        table,
        td {{
            border-collapse: collapse;
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }}

        img {{
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }}

        p {{
            display: block;
            margin: 13px 0;
        }}
        </style>
        <!--[if !mso]><!-->
        <style type=""text/css"">
        @media only screen and (max-width:480px) {{
            @-ms-viewport {{
                width: 320px;
            }}
            @viewport {{
                width: 320px;
            }}
        }}
        </style>
        <!--<![endif]-->
        <!--[if mso]>
        <xml>
        <o:OfficeDocumentSettings>
          <o:AllowPNG/>
          <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
        </xml>
        <![endif]-->
        <!--[if lte mso 11]>
        <style type=""text/css"">
          .outlook-group-fix {{ width:100% !important; }}
        </style>
        <![endif]-->


        <style type=""text/css"">
        @media only screen and (min-width:480px) {{
            .mj-column-per-100 {{
                width: 100% !important;
            }}
        }}
        </style>


        <style type=""text/css"">
        </style>

        </head>

        <body style=""background-color:#f9f9f9;"">


        <div style=""background-color:#f9f9f9;"">


        <!--[if mso | IE]>
        <table
        align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""
        >
        <tr>
          <td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
        <![endif]-->


        <div style=""background:#f9f9f9;background-color:#f9f9f9;Margin:0px auto;max-width:600px;"">

            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""background:#f9f9f9;background-color:#f9f9f9;width:100%;"">
                <tbody>
                    <tr>
                        <td style=""border-bottom:#F26302 solid 5px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;"">
                            <!--[if mso | IE]>
                  <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                
        <tr>

        </tr>

                  </table>
                <![endif]-->
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>


        <!--[if mso | IE]>
          </td>
        </tr>
        </table>

        <table
        align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""
        >
        <tr>
          <td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
        <![endif]-->


        <div style=""background:#fff;background-color:#fff;Margin:0px auto;max-width:600px;"">

            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""background:#fff;background-color:#fff;width:100%;"">
                <tbody>
                    <tr>
                        <td style=""border:#dddddd solid 1px;border-top:0px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;"">
                            <!--[if mso | IE]>
                  <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                
        <tr>

            <td
              style=""vertical-align:bottom;width:600px;""
            >
          <![endif]-->

                            <div class=""mj-column-per-100 outlook-group-fix"" style=""font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:bottom;width:100%;"">

                                <table border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""vertical-align:bottom;"" width=""100%"">

                                    <tr>
                                        <td align=""center"" style=""font-size:0px;padding:10px 25px;word-break:break-word;"">

                                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""border-collapse:collapse;border-spacing:0px;"">
                                                <tbody>
                                                    <tr>
                                                        <td style=""width:200px;"">

                                                        <img height=""auto"" src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/BottomLogo.jpeg?alt=media&token=48c6d297-9821-4d2c-bb7f-33ea079043f7"" style=""border:0;display:block;outline:none;text-decoration:none;width:100%;"" width=""200"" />

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align=""center"" style=""font-size:0px;padding:10px 25px;word-break:break-word;"">

                                            <div style=""font-family:'Helvetica Neue',Arial,sans-serif;font-size:24px;font-weight:bold;line-height:22px;text-align:center;color:#525252;"">
                                                Zoom Meeting Scheduled 
                                            </div>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align=""left"" style=""font-size:0px;padding:10px 25px;word-break:break-word;"">

                                            <div style=""font-family:'Helvetica Neue',Arial,sans-serif;font-size:14px;line-height:22px;text-align:left;color:#525252;"">
                                                <p>Hi {Username},</p>

                                                <p>You have scheduled Zoom Meeting having subject {title}</p>
                                            </div>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align=""left"" style=""font-size:0px;padding:10px 25px;word-break:break-word;"">

                                            <table 0=""[object Object]"" 1=""[object Object]"" 2=""[object Object]"" border=""0"" style=""cellspacing:0;color:#000;font-family:'Helvetica Neue',Arial,sans-serif;font-size:13px;line-height:22px;table-layout:auto;width:100%;"">
                                                <tr style=""border-bottom:1px solid #ecedee;text-align:left;"">
                                                    <th style=""padding: 15px 0px 15px 0;"">Title</th>
                                                    <th style=""padding: 15px 0px 15px 0px;"" align=""right"">When</th>
                                                </tr>
                                                <tr>
                                                    <td style=""padding: 15px 0px 15px 0;"">{title}</td>
                                                    <td style=""padding: 15px 0px 15px 0px;"" align=""right"">{Date} UTC</td>
                                                </tr>
                                                
                                                
                                            </table>

                                        </td>
                                    </tr>

                                    

                                    
                                    

                                    <tr>
                                        <td align=""center"" style=""font-size:0px;padding:10px 25px;padding-top:30px;padding-bottom:50px;word-break:break-word;"">

                                            <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""border-collapse:separate;line-height:100%;"">
                                                <tr>
                                                    <td align=""center""  role=""presentation"" style=""border:none;border-radius:3px;color:#ffffff;cursor:auto;padding:15px 25px;"" valign=""middle"">
                                                        <p 
                                                        style=""background: linear-gradient(to right,rgb(254,0,0), rgb(235,123,49), rgb(235,123,49), rgb(235,123,49) 30%, rgb(235,123,49) 70%, rgb(255,165,0)) !important;
                                                        color: #ffffff;
                                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                                        font-size: 15px;
                                                        font-weight: 800; /* Note: font-weight was already set to 800, so no need to set it again */
                                                        line-height: 120%;
                                                        margin: 0;
                                                        text-decoration: none;
                                                        text-transform: none;
                                                        box-shadow: 2px 2px 5px rgba(0,0,0,0.3); /* Shadow */
                                                        padding: 18px 20px 18px 20px; 
                                                        border-radius: 25px;""
                                                        >
                                                        <a href=""{url}"" style=""color:#fff; text-decoration:none"">Join Meeting Here</a>
                                                        </p>
                                                    </td>
                                                </tr>
                                            </table>

                                        </td>
                                    </tr>

                                    <tr>
                                        <td align=""left"" style=""font-size:0px;padding:10px 25px;word-break:break-word;"">

                                            <div style=""font-family:'Helvetica Neue',Arial,sans-serif;font-size:14px;line-height:20px;text-align:left;color:#525252;"">
                                                Best regards,
                                                <br><br> {Username}
                                                {title}
                                                <br>
                                                
                                            </div>

                                        </td>
                                    </tr>

                                </table>

                            </div>

                            <!--[if mso | IE]>
            </td>
          
        </tr>

                  </table>
                <![endif]-->
                        </td>
                    </tr>
                </tbody>
            </table>

        </div>


        <!--[if mso | IE]>
          </td>
        </tr>
        </table>

        <table
        align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""
        >
        <tr>
          <td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
        <![endif]-->


        <div style=""margin: 0px auto; max-width: 600px"">
        <table
          align=""center""
          border=""0""
          cellpadding=""0""
          cellspacing=""0""
          role=""presentation""
          style=""width: 100%""
        >
          <tbody>
            <tr>
              <td
                style=""
                  border-bottom: #F26302 solid 5px;
                  direction: ltr;
                  font-size: 0px;
                  padding: 20px 0;
                  text-align: center;
                  vertical-align: top;
                ""
              >
                <!--[if mso | IE]>
                  <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                
        <tr>

            <td
              style=""vertical-align:bottom;width:600px;""
            >
          <![endif]-->

                <div
                  class=""mj-column-per-100 outlook-group-fix""
                  style=""
                    font-size: 13px;
                    text-align: left;
                    direction: ltr;
                    display: inline-block;
                    vertical-align: bottom;
                    width: 100%;
                  ""
                >
                  <table
                    border=""0""
                    cellpadding=""0""
                    cellspacing=""0""
                    role=""presentation""
                    width=""100%""
                  >
                    <tbody>
                      <tr>
                        <td style=""vertical-align: bottom; padding: 0"">
                          <table
                            border=""0""
                            cellpadding=""0""
                            cellspacing=""0""
                            role=""presentation""
                            width=""100%""
                          >
                            <tr>
                              <td
                                align=""center""
                                style=""
                                  font-size: 0px;
                                  padding: 0;
                                  word-break: break-word;
                                ""
                              >
                                <div
                                  style=""
                                    font-family: 'Helvetica Neue', Arial,
                                      sans-serif;
                                    font-size: 12px;
                                    font-weight: 300;
                                    line-height: 1;
                                    text-align: center;
                                    color: #000000;
                                  ""
                                >
                                950 Dannon View, SW Suite 4103 Atlanta, GA 30331
                                </div>
                              </td>
                            </tr>

                            <tr>
                              <td
                                align=""center""
                                style=""
                                  font-size: 0px;
                                  padding: 10px;
                                  word-break: break-word;
                                ""
                              >
                                <div
                                  style=""
                                    font-family: 'Helvetica Neue', Arial,
                                      sans-serif;
                                    font-size: 12px;
                                    font-weight: 300;
                                    line-height: 1;
                                    text-align: center;
                                    color: #000000;
                                  ""
                                >
                                  <a href="""" style=""color: #000000""
                                    >Phone</a
                                  >
                                  404-593-0993
                                </div>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>

                <!--[if mso | IE]>
            </td>
          
        </tr>

                  </table>
                <![endif]-->
              </td>
            </tr>
          </tbody>
        </table>
        </div>


        <!--[if mso | IE]>
          </td>
        </tr>
        </table>
        <![endif]-->


        </div>

        </body>

        </html>
    ";
      return emailTemplate;
    }

  }
}
