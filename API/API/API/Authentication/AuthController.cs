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
        public AuthController
        (
          IHttpClientFactory httpClientFactory,
          IMapper mapper, ILogger<AuthController> logger,
          IAuthManager authManager,
          IPasswordReset_Service passwordReset_Service,
          IEmail_Service emailService,
          ITenants_Service tenants_Service
        )
        {

            this.mapper        = mapper;
            this.logger        = logger;
            this.authManager   = authManager;
            _httpClientFactory = httpClientFactory;
            _passwordService   = passwordReset_Service;
            _emailService      = emailService;
            _tenants_Service   = tenants_Service;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
        public async Task<ActionResult> GetTenants()
        {
            var authResponse = await _tenants_Service.GetAll();
            return Ok(authResponse);
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserModel apiUserDto)
        {
            string res = await authManager.RegisterCandidates(apiUserDto);
            if (res.StartsWith("OK"))
            {
                var successResponse = new SuccessResponse
                {
                    Message = "User Registered Successfully"
                };
                return Ok(successResponse);
            }
            return BadRequest(res);
           
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


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await authManager.FindbyEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                    return BadRequest(ModelState);
                }
                var otp = GenerateRandomOTP(8);
                var expirationTime = DateTime.Now.AddMinutes(15);

                // Create a PasswordResetRequest and store it
                var resetRequest = new PasswordResetDomain
                {
                    Email = model.Email,
                    ExpireTime = expirationTime,
                    OTP = otp
                };

                string retValue = await StorePasswordResetRequest(resetRequest);
                if (!retValue.StartsWith("OK"))
                {
                    return BadRequest(retValue);
                }
                string subject = "Password Reset OTP";
                string body = "<h3> This is your Password reset OTP Please don't share with anyone.</h3> \r\n\n\n  <h1>" + otp + "</h1> \r\n\n";
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
                    message = "No OTP against your email found";
                    return Content($"{{ \"message\": \"{message}\" }}", "application/json");
                }
                dynamic retVal = await authManager.VerifyOTP(model.Email, otp.OTP);
                if (retVal is int)
                {
                    string retValue = await authManager.UpdatePassord(user.Email, model.Password);
                    if (retValue.StartsWith("OK"))
                    {
                        string subject = "Password Changed Successfully";
                        string salutation = "Dear " + user.FirstName + " " + user.LastName;
                        string messageBody = $"<h1>Your Password changed recently thanks to be part of Pelican HRM </h1>";
                        string emailMessage = $"{subject}\n\n{salutation}\n\n{messageBody}";
                        await _emailService.SendEmailAsync(user.Email, subject, emailMessage);
                        bool res = await _passwordService.Delete(otp.Id);
                        if (res)
                        {
                            await _passwordService.CompleteAync();
                            message = "Your Password is  changed Successfully";
                            return Content($"{{ \"message\": \"{message}\" }}", "application/json");

                        }

                        return Content($"{{ \"message\": \"{retVal}\" }}", "application/json");
                    }

                    bool rese = await _passwordService.Delete(otp.Id);
                    await _passwordService.CompleteAync();
                    return Content($"{{ \"message\": \"{retVal}\" }}", "application/json");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer")]
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

        [HttpPut]
        [Route("UpdateCompany")]
        [CustomAuthorize("Update")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperUser")]
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


        [HttpPut]
        [Route("UpdateCompanyStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Developer,SuperUser")]
        public async Task<ActionResult> UpdateCompanyStatus(TenantBlock tenantUpdate)
        {

            try
            {
                var successResponse     = new SuccessResponse();
                successResponse.Message = await authManager.UpdateTenantStatus(tenantUpdate);
                if (successResponse.Message == "OK")
                {
                    successResponse.Message = "OK Compant status is  successfully Updated";
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
    }


}
