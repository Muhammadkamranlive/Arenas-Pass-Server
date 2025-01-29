using AutoMapper;
using System.Data;
using Server.Models;
using Server.Domain;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILogger<AuthController> logger;
        private readonly IAuthManager             authManager;
        private readonly IHttpClientFactory      _httpClientFactory;
        private readonly IPasswordReset_Service  _passwordService;
        private readonly IEmail_Service          _emailService;
        private readonly ITenants_Service        _tenants_Service;
        private readonly IPaymentService         _paymentService;
        public AuthController
        (
          IHttpClientFactory      httpClientFactory,
          IMapper                 mapper, 
          ILogger<AuthController> logger,
          IAuthManager            authManager,
          IPasswordReset_Service  passwordReset_Service,
          IEmail_Service          emailService,
          ITenants_Service        tenants_Service,
          IPaymentService         paymentService
        )
        {

            this.mapper        = mapper;
            this.logger        = logger;
            this.authManager   = authManager;
            _httpClientFactory = httpClientFactory;
            _passwordService   = passwordReset_Service;
            _emailService      = emailService;
            _tenants_Service   = tenants_Service;
            _paymentService    = paymentService;

        }


        [HttpPut]
        [Route("UpdateProfile")]
        [CustomAuthorize("Update")]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateUserModel loginDto)
        {
            var authResponse = await authManager.UpdateUser(loginDto);

            return Ok(authResponse);
        }

        [HttpGet]
        [Route("GetTenants")]
        [CustomAuthorize("Read")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin")]
        public async Task<ActionResult> GetTenants()
        {
            var authResponse = await _tenants_Service.GetAll();
            return Ok(authResponse);
        }

        

        [HttpPost]
        [Route("RegisterTenant")]
        public async Task<ActionResult> RegisterTenant([FromBody] TenantRegisterModel model)
        {
            
            var message   = await authManager.RegisterTenant( model);
            if (message.StartsWith("OK"))
            {
                var successResponse = new SuccessResponse
                {
                    Message = "Your Company has been Registered Successfully"
                };
                return Ok(successResponse);
            }
            var successResponse1 = new SuccessResponse
            {
                Message = message
            };
            return BadRequest(successResponse1);
        }

        [HttpGet]
        [Route("UpdatRole")]
        [CustomAuthorize("Update")]
        public async Task<ActionResult> UpdatRole()
        {
            var res= await authManager.UpdateRole();
            var successResponse = new SuccessResponse
            {
                Message = res
            };
            return Ok(successResponse);
        }

        [HttpPost]
        [Route("RegisterUsers")]
        [CustomAuthorize("Write")]
        public async Task<ActionResult> RegisterUsers([FromBody] AddUsersModel apiUserDto)
        {
            IList<IdentityError> errors = (IList<IdentityError>)await authManager.RegisterUsers(apiUserDto);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
            }
            var successResponse = new SuccessResponse
            {
                Message = "User Registered Successfully"
            };
            return Ok(successResponse);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterModel apiUserDto)
        {
            string errors = await authManager.Register(apiUserDto);
            if (!errors.StartsWith("OK"))
            {
                return BadRequest(errors);
            }
            var successResponse = new SuccessResponse
            {
                Message         = errors
            };
            return Ok(successResponse);
        }


        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterUserModel apiUserDto)
        {
            IList<IdentityError> errors = (IList<IdentityError>)await authManager.RegisterAdmin(apiUserDto);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
            }
            var successResponse = new SuccessResponse
            {
                Message = "User Registered Successfully"
            };
            return Ok(successResponse);
        }


        [HttpPost]
        [Route("RegisterSuperAdmin")]
        public async Task<ActionResult> RegisterSuperAdmin([FromBody] RegisterUserModel apiUserDto)
        {
            IList<IdentityError> errors = (IList<IdentityError>)await authManager.RegisterSuperAdmin(apiUserDto);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
            }
            var successResponse = new SuccessResponse
            {
                Message = "User Registered Successfully"
            };
            return Ok(successResponse);
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginModel loginDto)
        {

            try
            {
                return Ok(await authManager.Login(loginDto));
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }        

        [HttpGet]
        [Route("getById")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> getById(string uid)
        {
            try
            {
                var authResponse = await authManager.FindById(uid);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("SendComfirmEmail")]
        public async Task<ActionResult> SendComfirmEmail(ForgotPasswordModel model)
        {
            try
            {
                string message = await authManager.SendComfirmEmail(model.Email);
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while sending the confirmation email." + ex.Message);
            }
        }
        [HttpPost]
        [Route("ComfirmEmail")]
        public async Task<ActionResult> ComfirmEmail(ConfirmEmail model)
        {
            try
            {
                string message = await authManager.ComfirmEmail(model.Email, model.OTP);
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while sending the confirmation email." + ex.Message);
            }
        }

        [HttpPost]
        [Route("Enable2FA")]
        public async Task<ActionResult> Enable2FA(ConfirmEmail model)
        {
            try
            {
                string message = await authManager.EnableTwoFactorAuthEmail(model.Email, model.OTP);
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while sending the confirmation email." + ex.Message);
            }
        }

        [HttpPost]
        [Route("Disable2FA")]
        public async Task<ActionResult> Disable2FA(ConfirmEmail model)
        {
            try
            {
                string message = await authManager.DisableTwoFactorAuthEmail(model.Email, model.OTP);
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while sending the confirmation email." + ex.Message);
            }
        }

        [HttpPost]
        [Route("VerifyEnable2FA")]
        public async Task<ActionResult> VerifyEnable2FA(ConfirmEmail model)
        {
            try
            {
                string message = await authManager.VerifyTwoFactorAuthEmail(model.Email, model.OTP);
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while sending the confirmation email." + ex.Message);
            }
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await authManager.FindbyEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    return BadRequest(ModelState);
                }
                var otp            = GenerateRandomOTP(8);
                var expirationTime = DateTime.Now.AddMinutes(15);

                // Create a PasswordResetRequest and store it
                var resetRequest = new PasswordResetDomain
                {
                    Email      = model.Email,
                    ExpireTime = expirationTime,
                    OTP        = otp
                };

                string retValue = await StorePasswordResetRequest(resetRequest);
                if (!retValue.StartsWith("OK"))
                {
                    return BadRequest(retValue);
                }
                string subject = "Password Reset OTP";
                string name    = user.FirstName +" "+ user.LastName;
                string body    = CreatePasswordForgotEmailBody(otp,name);
                
                retValue = await SendPasswordResetOTPEmailAsync(model.Email, subject, body);
                if (!retValue.StartsWith("OK"))
                {
                    return BadRequest(retValue);
                }
                var message = "Password reset instructions sent to your email.";
                return Content($"{{ \"message\": \"{message}\" }}", "application/json");
            }

            return BadRequest(ModelState);
        }

        [HttpPost("RestPassword")]
        public async Task<IActionResult> RestPassword(ResetModel model)
        {
            string message;
            if (ModelState.IsValid)
            {
                ApplicationUser user = await authManager.FindbyEmail(model.Email);
                if (user == null)
                {

                    return BadRequest("User not found");
                }
                IList<PasswordResetDomain> otplist = (IList<PasswordResetDomain>)await _passwordService.GetAll();
                if (otplist.Count == 0)
                {
                    return BadRequest("No OTP Found");
                }
                PasswordResetDomain otp = otplist.Where(x => x.Email == model.Email).OrderBy(x => x.ExpireTime).LastOrDefault();
                if (otp == null)
                {
                    message       = "No OTP against your email found";
                    return Content($"{{ \"message\": \"{message}\" }}", "application/json");
                }
                dynamic retVal = await authManager.VerifyOTP(model.Email, otp.OTP);
                if (retVal is Guid)
                {
                    string retValue = await authManager.UpdatePassord(user.Email, model.Password);
                    if (retValue.StartsWith("OK"))
                    {
                        string subject      = "Password Changed Successfully";
                        string salutation   = user.FirstName + " " + user.LastName;
                 
                        string emailMessage = CreatePasswordResetEmailBody(salutation);
                        await _emailService.SendEmailAsync(user.Email, subject, emailMessage);
                        bool res = await _passwordService.Delete(otp.Id);
                        if (res)
                        {
                            await _passwordService.CompleteAync();
                            message = "Your Password is  changed Successfully";
                            return Content($"{{ \"message\": \"{message}\" }}", "application/json");

                        }

                       
                    }
                    else
                    {
                        message = retValue;
                        return BadRequest(message);
                    }

                }
                bool resee = await _passwordService.Delete(otp.Id);
                await _passwordService.CompleteAync();
                message = "Your Password is  changed Successfully";
                return Content($"{{ \"message\": \"{retVal}\" }}", "application/json");
            }

            return BadRequest(ModelState);
        }

        private async Task<string> SendPasswordResetOTPEmailAsync(string to, string subject, string content)
        {

            await _emailService.SendEmailAsync(to, subject, content, isHtml: true);
            return "OK";
        }
        private async Task<string> StorePasswordResetRequest(PasswordResetDomain resetRequest)
        {

            try
            {

                await _passwordService.InsertAsync(resetRequest);
                await _passwordService.CompleteAync();
                var message = "OK";
                return message;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }



        }
        private static string GenerateRandomOTP(int length)
        {
            const string validChars = "0123456789";
            var random = new Random();
            var otp = new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return otp;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> GetAllRoles()
        {
            try
            {
                var roles = await authManager.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("AddRoles")]
        [CustomAuthorize("Write")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin")]
        public async Task<ActionResult> CreateRole(AddRole Role)
        {
            try
            {
                var errors = await authManager.CreateRole(Role.Role,Role.Permissions);
                if (errors != null && errors.Any())
                {
                    return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
                }
                return CreateJsonResponse($"Role '{Role.Role}' created successfully.");
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateRole")]
        [CustomAuthorize("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin")]
        public async Task<ActionResult> UpdateRole(UpdateRoleModel model)
        {
            try
            {
                var errors = await authManager.UpdateUserRole(model.uid, model.role);
                if (errors != null && errors.Any())
                {
                    return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
                }
                return CreateJsonResponse($"Role '{model.uid}' updated to '{model.role}' successfully.");
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("UpdateRoleAndPermission")]
        [CustomAuthorize("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin")]
        public async Task<ActionResult> UpdateRoleAndPermission(UpdateRoleAndPermissionModel model)
        {
            try
            {
                var errors = await authManager.UpdateRoleAndPermissions(model.Id, model.Role,model.Permissions);
                if (errors != null && errors.Any())
                {
                    return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
                }
                return CreateJsonResponse($"Role '{model.Id}' updated to '{model.Role}' successfully.");
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("UpdateCRMuser")]
        [CustomAuthorize("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,Merchant,SuperAdmin")]
        public async Task<ActionResult> UpdateCRMuser(AddUsersModel model)
        {
            try
            {
                
                return CreateJsonResponse(await authManager.UpdateCRMUser(model));
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("deleteRole")]
        [CustomAuthorize("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            try
            {
                var errors = await authManager.DeleteRole(id);
                if (errors != null && errors.Any())
                {
                    return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
                }

                return CreateJsonResponse($"Role '{id}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [CustomAuthorize("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,Merchant,SuperAdmin")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                var errors  = await authManager.DeleteUser(id);
                if (errors != null && errors.Any())
                {
                    return BadRequest(errors.Select(x => x.Description).FirstOrDefault());
                }

                return CreateJsonResponse($"User '{id}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return CreateJsonResponse($"Internal server error: {ex.Message}");
            }
        }

        private ContentResult CreateJsonResponse(string param)
        {
            var jsonResponse = new
            {
                message = param
            };

            var message = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse);

            return Content(message, "application/json");
        }

        

        [HttpGet]
        [Route("GetUserWithRoles")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> GetUserWithRoles()
        {
            var users = await authManager.GetAllUsersWithRoles();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetMerchants")]
        [CustomAuthorize("Read")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,SuperAdmin")]
        public async Task<IActionResult> GetMerchants()
        {
            var users = await authManager.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetByusername")]
        public async Task<ActionResult> GetByusername(string username)
        {

            try
            {
                var successResponse     = new SuccessResponse();
                successResponse.Message = await authManager.GetCompanyByName(username);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllTenants")]
        public async Task<ActionResult> GetAllTenants()
        {

            try
            {


               var list= await authManager.GetTenants();
                return Ok(list);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetTenantDetail")]
        public async Task<ActionResult> GetTenantDetail(string OnwerId)
        {

            try
            {

                dynamic list = await authManager.GetTenantsDetailbyId(OnwerId);
                if(list is string)
                {
                    return BadRequest("User Not Found");
                }
                return Ok(list);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("UpdateCompany")]
        [CustomAuthorize("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,Merchant,SuperAdmin")]
        public async Task<ActionResult> UpdateCompany(TenantUpdate tenantUpdate)
        {

            try
            {
                var successResponse     = new SuccessResponse();
                successResponse.Message = await authManager.UpdateTenant(tenantUpdate);
                if (successResponse.Message == "OK")
                {
                    successResponse.Message = "OK your company has been registered Successfully";
                    return Ok(successResponse);
                }
                else
                {
                    return BadRequest(successResponse);
                }
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("getPaymentsClient")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> getPaymentsClient(string uid)
        {
            try
            {
                ApplicationUser authResponse     = await authManager.FindById(uid);
                IList<PaymentSession> payments   = await _paymentService.GetClientPaymentSession();
                payments                         = payments.Where(x=>x.CustomerEmail==authResponse.Email).ToList();
                return Ok(payments);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("GetUpdateTenant")]
        public async Task<ActionResult> GetUpdateTenant()
        {
            try
            {
                var list = await authManager.GetUpdateTenant();
                return Ok(list);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpPut]
        [Route("SendPasswordResetToUser")]
        public async Task<ActionResult> SendPasswordResetToUser(IdModel Id)
        {
            try
            {
                var successResponse      = new SuccessResponse();
                successResponse.Message  = await authManager.SendResetPassword(Id.Id);
                if (successResponse.Message.StartsWith("OK"))
                {
                    return Ok(successResponse);
                }
                else
                {
                    return BadRequest(successResponse);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }



        [HttpPut]
        [Route("UpdateCompanyStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperAdmin,Merchant")]
        public async Task<ActionResult> UpdateCompanyStatus(TenantBlock tenantUpdate)
        {

            try
            {
                var successResponse     = new SuccessResponse();
                successResponse.Message = await authManager.UpdateTenantStatus(tenantUpdate);
                if (successResponse.Message == "OK")
                {
                    successResponse.Message = "OK Company status is  successfully Updated";
                    return Ok(successResponse);
                }
                else
                {
                    return BadRequest(successResponse);
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        private static string CreatePasswordResetEmailBody(string Username)
        {
            string emailTemplate = $@"
            <!DOCTYPE html>
            <html
              xmlns=""http://www.w3.org/1999/xhtml""
              xmlns:v=""urn:schemas-microsoft-com:vml""
              xmlns:o=""urn:schemas-microsoft-com:office:office""
            >
              <head>
                <title> </title>
                <!--[if !mso]><!-- -->
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
                <!--<![endif]-->
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
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
   
                <style type=""text/css"">
                  @media only screen and (max-width: 480px) {{
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
                      <o:AllowPNG />
                      <o:PixelsPerInch>96</o:PixelsPerInch>
                    </o:OfficeDocumentSettings>
                  </xml>
                <![endif]-->
                <!--[if lte mso 11]>
                  <style type=""text/css"">
                    .outlook-group-fix {{
                      width: 100% !important;
                    }}
                  </style>
                <![endif]-->

                <style type=""text/css"">
                  @media only screen and (min-width: 480px) {{
                    .mj-column-per-100 {{
                      width: 100% !important;
                    }}
                  }}
                </style>

                <style type=""text/css""></style>
              </head>

              <body style=""background-color: #f9f9f9"">
                <div style=""background-color: #f9f9f9"">
                  <!--[if mso | IE]>
                  <table
                     align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""
                  >
                    <tr>
                      <td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
                  <![endif]-->

                  <div
                    style=""
                      background: rgb(241,245,249);
                      background-color: rgb(241,245,249);
                      margin: 0px auto;
                      max-width: 600px;
                    ""
                  >
                    <table
                      align=""center""
                      border=""0""
                      cellpadding=""0""
                      cellspacing=""0""
                      role=""presentation""
                      style=""background: #f9f9f9; background-color: #f9f9f9; width: 100%""
                    >
                      <tbody>
                        <tr>
                          <td
                            style=""
                              border-bottom: #5d16ec solid 5px;
                              direction: ltr;
                              font-size: 0px;
                              padding: 20px 0;
                              text-align: center;
                              vertical-align: top;
                            ""
                          >
                            <!--[if mso | IE]>
                              <table
                                role=""presentation""
                                border=""0""
                                cellpadding=""0""
                                cellspacing=""0""
                              >
                                <tr></tr>
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

                  <div
                    style=""
                      background: #fff;
                      background-color: #fff;
                      margin: 0px auto;
                      max-width: 600px;
                    ""
                  >
                    <table
                      align=""center""
                      border=""0""
                      cellpadding=""0""
                      cellspacing=""0""
                      role=""presentation""
                      style=""background: #fff; background-color: #fff; width: 100%""
                    >
                      <tbody>
                        <tr>
                          <td
                            style=""
                              border: #dddddd solid 1px;
                              border-top: 0px;
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
                                style=""vertical-align: bottom""
                                width=""100%""
                              >
                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <table
                                      align=""center""
                                      border=""0""
                                      cellpadding=""0""
                                      cellspacing=""0""
                                      role=""presentation""
                                      style=""border-collapse: collapse; border-spacing: 0px""
                                    >
                                      <tbody>
                                        <tr>
                                          <td style=""width: 200px"">
                                            <img
                                              height=""auto""
                                              src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Alogo.jpg?alt=media&token=99ef0c51-bc96-4929-9f84-b0613d348f9b""
                                              style=
                                              ""
                                              border: 0;
                                              display: block;
                                              outline: none;
                                              text-decoration: none;
                                              width: 100%;
                                              ""
                                              width=""200""
                                            />
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 40px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 22px;
                                        font-weight: bold;
                                        line-height: 1;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                    Your password is changed successfully.
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 0;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                     Dear {Username}
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                    Your password is changed recently thanks to be part of PelicanHRM.
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 20px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                     Login into PelicanHRM
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-top: 30px;
                                      padding-bottom: 40px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <table
                                      align=""center""
                                      border=""0""
                                      cellpadding=""0""
                                      cellspacing=""0""
                                      role=""presentation""
                                      style=""border-collapse: separate; line-height: 100%""
                                    >
                                      <tr>
                                        <td
                                          align=""center""
                             
                                          role=""presentation""
                                          style=""
                                            border: none;
                                            border-radius: 3px;
                                            color: #ffffff;
                                            cursor: auto;
                                            padding: 15px 75px;
                                            border-radius: 25px;
                                          ""
                                          valign=""middle""
                                        >
                                        <a
                                        href=""https://pelicanhrm.com""
                                        style=""
                                        background: linear-gradient(to right,#31AFEA, #31AFEA, #31AFEA, #31AFEA 30%, #2F6FEA 70%, #2F6FEA) !important;
                                        color: #ffffff;
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 15px;
                                        font-weight: 800; /* Note: font-weight was already set to 800, so no need to set it again */
                                        line-height: 120%;
                                        margin: 0;
                                        text-decoration: none;
                                        text-transform: none;
                                        box-shadow: 2px 2px 5px rgba(0,0,0,0.3); /* Shadow */
                                        padding: 18px 70px 18px 70px; 
                                        border-radius: 25px;
                                        "">
                                         Login
                                        </a>
                        
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>

                    
                   
                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 26px;
                                        font-weight: bold;
                                        line-height: 1;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                      Need Help?
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 14px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                      Please send and feedback or bug info<br />
                                      to
                                      <a
                                        href=""mailto:info@example.com""
                                        style=""color: #2f67f6""
                                        >info@pelicanhrm.com</a
                                      >
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
                              border-bottom: #5d16ec solid 5px;
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
        private static string CreatePasswordForgotEmailBody(string OTP, string Username)
        {
            string emailTemplate = $@"
               <!DOCTYPE html>
            <html
              xmlns=""http://www.w3.org/1999/xhtml""
              xmlns:v=""urn:schemas-microsoft-com:vml""
              xmlns:o=""urn:schemas-microsoft-com:office:office""
            >
              <head>
                <title> </title>
                <!--[if !mso]><!-- -->
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
                <!--<![endif]-->
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"" />
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
                  @media only screen and (max-width: 480px) {{
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
                      <o:AllowPNG />
                      <o:PixelsPerInch>96</o:PixelsPerInch>
                    </o:OfficeDocumentSettings>
                  </xml>
                <![endif]-->
                <!--[if lte mso 11]>
                  <style type=""text/css"">
                    .outlook-group-fix {{
                      width: 100% !important;
                    }}
                  </style>
                <![endif]-->

                <style type=""text/css"">
                  @media only screen and (min-width: 480px) {{
                    .mj-column-per-100 {{
                      width: 100% !important;
                    }}
                  }}
                </style>

                <style type=""text/css""></style>
              </head>

              <body style=""background-color: #f9f9f9"">
                <div style=""background-color: #f9f9f9"">
                  <!--[if mso | IE]>
                  <table
                     align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:600px;"" width=""600""
                  >
                    <tr>
                      <td style=""line-height:0px;font-size:0px;mso-line-height-rule:exactly;"">
                  <![endif]-->

                  <div
                    style=""
                      background: rgb(241,245,249);
                      background-color: rgb(241,245,249);
                      margin: 0px auto;
                      max-width: 600px;
                    ""
                  >
                    <table
                      align=""center""
                      border=""0""
                      cellpadding=""0""
                      cellspacing=""0""
                      role=""presentation""
                      style=""background: #f9f9f9; background-color: #f9f9f9; width: 100%""
                    >
                      <tbody>
                        <tr>
                          <td
                            style=""
                              border-bottom: #5d16ec solid 5px;
                              direction: ltr;
                              font-size: 0px;
                              padding: 20px 0;
                              text-align: center;
                              vertical-align: top;
                            ""
                          >
                            <!--[if mso | IE]>
                              <table
                                role=""presentation""
                                border=""0""
                                cellpadding=""0""
                                cellspacing=""0""
                              >
                                <tr></tr>
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

                  <div
                    style=""
                      background: #fff;
                      background-color: #fff;
                      margin: 0px auto;
                      max-width: 600px;
                    ""
                  >
                    <table
                      align=""center""
                      border=""0""
                      cellpadding=""0""
                      cellspacing=""0""
                      role=""presentation""
                      style=""background: #fff; background-color: #fff; width: 100%""
                    >
                      <tbody>
                        <tr>
                          <td
                            style=""
                              border: #dddddd solid 1px;
                              border-top: 0px;
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
                                style=""vertical-align: bottom""
                                width=""100%""
                              >
                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <table
                                      align=""center""
                                      border=""0""
                                      cellpadding=""0""
                                      cellspacing=""0""
                                      role=""presentation""
                                      style=""border-collapse: collapse; border-spacing: 0px""
                                    >
                                      <tbody>
                                        <tr>
                                          <td style=""width: 200px"">
                                            <img
                                              height=""auto""
                                              src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/Alogo.jpg?alt=media&token=99ef0c51-bc96-4929-9f84-b0613d348f9b""
                                              style=
                                              ""
                                              border: 0;
                                              display: block;
                                              outline: none;
                                              text-decoration: none;
                                              width: 100%;
                                              ""
                                              width=""200""
                                            />
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 40px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 32px;
                                        font-weight: bold;
                                        line-height: 1;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                     Forgot Password ?
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 0;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                     Dear {Username}
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                    This is your password reset OTP (One-Time Password) . <br> Please don't share it with anyone.
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-bottom: 20px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 16px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                     OTP (One-Time Password)
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      padding-top: 30px;
                                      padding-bottom: 40px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <table
                                      align=""center""
                                      border=""0""
                                      cellpadding=""0""
                                      cellspacing=""0""
                                      role=""presentation""
                                      style=""border-collapse: separate; line-height: 100%""
                                    >
                                      <tr>
                                        <td
                                          align=""center""
                             
                                          role=""presentation""
                                          style=""
                                            border: none;
                                            border-radius: 3px;
                                            color: #ffffff;
                                            cursor: auto;
                                            padding: 15px 75px;
                                            border-radius: 25px;
                                          ""
                                          valign=""middle""
                                        >
                                        <p style=""
                                        background: linear-gradient(to right,#31AFEA, #31AFEA, #31AFEA, #31AFEA 30%, #2F6FEA 70%, #2F6FEA) !important;
                                        color: #ffffff;
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 15px;
                                        font-weight: 800; /* Note: font-weight was already set to 800, so no need to set it again */
                                        line-height: 120%;
                                        margin: 0;
                                        text-decoration: none;
                                        text-transform: none;
                                        box-shadow: 2px 2px 5px rgba(0,0,0,0.3); /* Shadow */
                                        padding: 18px 70px 18px 70px; 
                                        border-radius: 25px;
                                        "">
                                        {OTP}
                                        </p>
                        
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                                </tr>

                    
                   
                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 26px;
                                        font-weight: bold;
                                        line-height: 1;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                      Need Help?
                                    </div>
                                  </td>
                                </tr>

                                <tr>
                                  <td
                                    align=""center""
                                    style=""
                                      font-size: 0px;
                                      padding: 10px 25px;
                                      word-break: break-word;
                                    ""
                                  >
                                    <div
                                      style=""
                                        font-family: 'Helvetica Neue', Arial, sans-serif;
                                        font-size: 14px;
                                        line-height: 22px;
                                        text-align: center;
                                        color: #555;
                                      ""
                                    >
                                      Please send and feedback or bug info<br />
                                      to
                                      <a
                                        href=""mailto:info@example.com""
                                        style=""color: #2f67f6""
                                        >info@pelicanhrm.com</a
                                      >
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
                              border-bottom: #5d16ec solid 5px;
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
