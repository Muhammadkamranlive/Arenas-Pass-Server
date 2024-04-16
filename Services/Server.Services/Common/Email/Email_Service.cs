using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Server.Services
{
    public class Email_Service : IEmail_Service
    {
        public Email_Service()
        {
                
        }
        public async Task SendEmailAsync(string to, string subject, string content, bool isHtml = true)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Pelican HRM", "Muhammadkamranntu@gmail.com"));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var emailBody = CreateEmailBody(content);

            var textPart = new TextPart(TextFormat.Html)
            {
                Text = emailBody
            };

            message.Body = textPart;

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("Muhammadkamranntu@gmail.com", "dmwd xobz dpph ipvg");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        public async Task SendEmail1Async(string to, string subject, string content, bool isHtml = true)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Pelican HRM", "donotreply@pelicanhrm.com")); // Update with your Roundcube email
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var emailBody = content;

            var textPart  = new TextPart(TextFormat.Html)
            {
                Text = emailBody
            };

            message.Body = textPart;

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("p3plzcpnl494306.prod.phx3.secureserver.net", 587, SecureSocketOptions.StartTls); // Update with Roundcube SMTP server and port                                                                                                           // Note: Roundcube may use different authentication methods, you may need to adjust this part accordingly
                    await client.AuthenticateAsync("oaw1qck8lht6", "Carpenter50##50"); 
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }
        public async Task SendBulkEmailAsync(List<string> toList, string subject, string content, bool isHtml = true)
        {
            foreach (var to in toList)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Emtiyaz EDU", "Emtiyaz-devTeam@outlook.com"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var emailBody = content;

                var textPart = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = emailBody
                };

                message.Body = textPart;

                using (var client = new SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync("Emtiyaz-devTeam@outlook.com", "16Feb@1234TK");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
            }
        }
        private static string CreateEmailBody(string content)
        {
            string emailTemplate = $@"
                 <!DOCTYPE HTML
                  PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
                <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml""
                  xmlns:o=""urn:schemas-microsoft-com:office:office"">

                <head>
                  <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                  <meta name=""x-apple-disable-message-reformatting"">
                  <!--[if !mso]><!-->
                  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!--<![endif]-->
                  <title></title>

                  <style type=""text/css"">
                    @media only screen and (min-width: 620px) {{
                      .u-row {{
                        width: 600px !important;
                      }}

                      .u-row .u-col {{
                        vertical-align: top;
                      }}

                      .u-row .u-col-33p33 {{
                        width: 199.98px !important;
                      }}

                      .u-row .u-col-100 {{
                        width: 600px !important;
                      }}

                    }}

                    @media (max-width: 620px) {{
                      .u-row-container {{
                        max-width: 100% !important;
                        padding-left: 0px !important;
                        padding-right: 0px !important;
                      }}

                      .u-row .u-col {{
                        min-width: 320px !important;
                        max-width: 100% !important;
                        display: block !important;
                      }}

                      .u-row {{
                        width: 100% !important;
                      }}

                      .u-col {{
                        width: 100% !important;
                      }}

                      .u-col>div {{
                        margin: 0 auto;
                      }}
                    }}

                    body {{
                      margin: 0;
                      padding: 0;
                    }}

                    table,
                    tr,
                    td {{
                      vertical-align: top;
                      border-collapse: collapse;
                    }}

                    p {{
                      margin: 0;
                    }}

                    .ie-container table,
                    .mso-container table {{
                      table-layout: fixed;
                    }}

                    * {{
                      line-height: inherit;
                    }}

                    a[x-apple-data-detectors='true'] {{
                      color: inherit !important;
                      text-decoration: none !important;
                    }}

                    @media (max-width: 480px) {{
                      .hide-mobile {{
                        max-height: 0px;
                        overflow: hidden;
                        display: none !important;
                      }}
                    }}

                    table,
                    td {{
                      color: #000000;
                    }}

                    #u_body a {{
                      color: #0000ee;
                      text-decoration: underline;
                    }}
                  </style>



                  <!--[if !mso]><!-->
                  <link href=""https://fonts.googleapis.com/css?family=Montserrat:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                  <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap"" rel=""stylesheet"" type=""text/css"">
                  <!--<![endif]-->

                </head>

                <body class=""clean-body u_body""
                  style=""margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #f0f0f0;color: #000000"">
                  <table id=""u_body""
                    style=""border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #f0f0f0;width:100%""
                    cellpadding=""0"" cellspacing=""0"">
                    <tbody>
                      <tr style=""vertical-align: top"">
                        <td style=""word-break: break-word;border-collapse: collapse !important;vertical-align: top"">
                          <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td align=""center"" style=""background-color: #f0f0f0;""><![endif]-->
                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ddffe7;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div style=""background-color: #fff;height: 100%;width: 100% !important;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;"">
                                      <!--<![endif]-->

                                      <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                        cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;""
                                              align=""left"">

                                              <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">
                                                <tr>
                                                  <td style=""padding-right: 0px;padding-left: 0px;"" align=""center"">

                                                    <img align=""center"" border=""0"" src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/WhatsApp%20Image%202024-01-14%20at%2011.50.43%20PM%20(3).jpeg?alt=media&token=4198b5e7-000a-45d4-8cf4-319314614e36"" alt=""image"" title=""image""
                                                      style=""outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 100%;max-width: 190px;""
                                                      width=""190"" />

                                                  </td>
                                                </tr>
                                              </table>

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>





                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->
                                      <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                        cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td
                                             style=""text-align:center; padding: 20px;""  
                                            >
                                              ${content}
                              

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>


                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->

                                      <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                        cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td>
                                              <h4 style=""text-align: center;"">
                                                PELICANHRM
                                              </h4>
                                              <h5 style=""text-align: center;"">
                                               Holding, Handling & Managing All Human Resource Experiences
                                              </h5>
                                            </td>
                                          </tr>
                                          <tr>
                                            <td
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                              align=""left"">

                                              <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                              <p
                                                style=""padding: 30px;"">
                                               Optimize Your HR Operations with Pelican HRM.
                                              </p>
                                              <!--[if mso]></td></tr></table><![endif]-->

                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>





                          <div class=""u-row-container"" style=""padding: 2px 0px 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 2px 0px 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""background-color: #ffffff;width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""background-color: #ffffff;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->

                                      <table style=""font-family:arial,helvetica,sans-serif;"" role=""presentation"" cellpadding=""0""
                                        cellspacing=""0"" width=""100%"" border=""0"">
                                        <tbody>
                                          <tr>
                                            <td
                                              style=""overflow-wrap:break-word;word-break:break-word;padding:30px 10px 10px;font-family:arial,helvetica,sans-serif;""
                                              align=""left"">

                                              <!--[if mso]><table width=""100%""><tr><td><![endif]-->
                                              <h1
                                                style=""margin: 0px; line-height: 140%; text-align: center; word-wrap: break-word; font-family: 'Montserrat',sans-serif; font-size: 13px; font-weight: 400;"">
                                                <span><span><span><span>If you have any questions, contact our Website  Guides.<br />Or,
                                                        visit our Help Center.</span></span></span></span></h1>
                                              <!--[if mso]></td></tr></table><![endif]-->

                                            </td>
                                          </tr>
                                          <tr >
                                            <td style=""align-items: center; padding-bottom: 20px; padding-top: 20px;"">
                                              <p style=""text-align: center; padding-top: 20px; padding-bottom: 20px;"">PelicanHRM.com | All Rights Reserved</p>
                                            </td>
                                          </tr>
                                        </tbody>
                                      </table>

                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>





                          <div class=""u-row-container"" style=""padding: 0px;background-color: transparent"">
                            <div class=""u-row""
                              style=""margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;"">
                              <div
                                style=""border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;"">
                                <!--[if (mso)|(IE)]><table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0""><tr><td style=""padding: 0px;background-color: transparent;"" align=""center""><table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width:600px;""><tr style=""background-color: transparent;""><![endif]-->

                                <!--[if (mso)|(IE)]><td align=""center"" width=""600"" style=""width: 600px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"" valign=""top""><![endif]-->
                                <div class=""u-col u-col-100""
                                  style=""max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;"">
                                  <div
                                    style=""height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                    <!--[if (!mso)&(!IE)]><!-->
                                    <div
                                      style=""box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;"">
                                      <!--<![endif]-->

                     
                                      <!--[if (!mso)&(!IE)]><!-->
                                    </div><!--<![endif]-->
                                  </div>
                                </div>
                                <!--[if (mso)|(IE)]></td><![endif]-->
                                <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]-->
                              </div>
                            </div>
                          </div>



                          <!--[if (mso)|(IE)]></td></tr></table><![endif]-->
                        </td>
                      </tr>
                    </tbody>
                  </table>
                  <!--[if mso]></div><![endif]-->
                  <!--[if IE]></div><![endif]-->
                </body>

                </html>";
            return emailTemplate;
        }


    }
}
