using AutoMapper;
using Server.Core;
using System.Text;
using Server.Domain;
using Server.Models;
using System.Text.Json;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Server.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace Server.Services
{
    public class AuthManager : IAuthManager
    {

        #region Fields
        private readonly IMapper                      _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration               _configuration;
        private readonly RoleManager<CustomRole>      _roleManager;
        private readonly IEmail_Service               _emailService;
        private readonly IPasswordReset_Service       _passwordResetService;
        private readonly IGeneralTask_Service         _generalTask_Service;
        private readonly INotifications_Service       _notifications_Service;
        private readonly ITenants_Service             _tenants_Service;
        private readonly ERPDb _context;
        private readonly IHttpContextAccessor        _httpContextAccessor;
        private readonly IPaymentService             _paymentService;
        #endregion

        #region Constructor
        public AuthManager
        (
            ERPDb context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            RoleManager<CustomRole> roleManager,
            IEmail_Service emailService,
            IPasswordReset_Service passwordResetService,
            IGeneralTask_Service generalTask_Service,
            INotifications_Service notifications_Service,
            ITenants_Service    tenants_Service,
            IHttpContextAccessor  httpContextAccessor,
            IPaymentService paymentService      


        )
        {
            _context               = context;
            _mapper                = mapper;
            _userManager           = userManager;
            _configuration         = configuration;
            _roleManager           = roleManager;
            _emailService          = emailService;
            _passwordResetService  = passwordResetService;
            _generalTask_Service   = generalTask_Service;
            _notifications_Service = notifications_Service;
            _tenants_Service       = tenants_Service;
            _httpContextAccessor   = httpContextAccessor;
            _paymentService        = paymentService;

        }

        #endregion 

        public async Task<AuthResponseModel> Login(UserLoginModel loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new AuthResponseModel { Message = "User not found." };
                }
                bool isValidUser = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!isValidUser)
                {
                    return new AuthResponseModel { Message = "Invalid credentials." };
                }
                IList<PaymentSession> Payments  = await _paymentService.GetClientPaymentSession();
                bool paid = false;
                if (Payments.Count > 0)
                {
                    var myPayment= Payments.Where(x=>x.CompanyName.ToLower()==user.CompanyName.ToLower()).FirstOrDefault();
                    if(myPayment!= null)
                    {
                        paid    = true;
                    }
                }
                var Tenants          = await _tenants_Service.Find(x => x.CompanyName.ToLower() == user.CompanyName.ToLower());
                int TenantId         = 1;
                string CompanyStatus = "Active";
                string ProfileStatus = "InCompleted";
                if (Tenants.Count != 0)
                {
                    PelicanHRMTenant tenant = Tenants.FirstOrDefault();
                    if(tenant!= null) 
                    {
                        
                        TenantId      = tenant.CompanyId;
                        CompanyStatus = tenant.CompanyStatus;
                        ProfileStatus = tenant.ProfileStatus;
                    }
                }
                var token =  await GenerateToken(user, TenantId, CompanyStatus, ProfileStatus, paid);
                return new AuthResponseModel
                {
                    Token            = token,
                    UserId           = user.Id,
                    EmailStatus      = user.EmailConfirmed,
                    CompanyStatus    = CompanyStatus,
                    ProfileStatus    = ProfileStatus,
                    paid             = paid,
                    Message          = "Success",
                    Name             = user.FirstName+" "+user.LastName,
                    CompanyName      = user.CompanyName,
                    TwoFactorEnabled = user.TwoFactorEnabled

                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<IdentityError>> RegisterAdmin(RegisterUserModel adminDto)
        {
            try
            {
                var admin                = new ApplicationUser();
                admin.FirstName          = adminDto.FirstName;
                admin.LastName           = adminDto.LastName;
                admin.MiddleName         = adminDto.MiddleName;
                admin.Email              = adminDto.Email; ;
                admin.UserName           = adminDto.Email;
                admin.EmployeeId         = await getEmployeeId() + 1;
                admin.TenantId           = 4;
                admin.CompanyDesignation = "Administrator";
                admin.CompanyName        = "PelicanHRM";
                var result = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("Administrator");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = "Administrator", Permissions = "Read,Write,Delete,Update" });
                    }

                    await _userManager.AddToRoleAsync(admin, "Administrator");
                   
                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<string> RegisterCandidates(RegisterUserModel model)
        {
            try
            {

                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x => x.CompanyName.ToLower() == model.CompanyName.ToLower());
                if (list.Count !=0)
                {
                  var admin                = new ApplicationUser();
                  PelicanHRMTenant company = list.FirstOrDefault();
                  admin.FirstName          = model.FirstName;
                  admin.LastName           = model.LastName;
                  admin.MiddleName         = model.MiddleName;
                  admin.Email              = model.Email;
                  admin.UserName           = model.Email;
                  admin.CompanyName        = company.CompanyName;
                  admin.EmployeeId         = await getEmployeeId() + 1; ;
                  admin.CompanyDesignation = "Employee";
                  admin.TenantId           = company.CompanyId;
                  admin.image              = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/images.jfif?alt=media&token=09284390-5fd1-40f7-b91c-feabadf143a9";
                  var result               = await _userManager.CreateAsync(admin, model.Password);
                  if (result.Succeeded)
                  {
                    var roleExists = await _roleManager.RoleExistsAsync("Employee");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = "Employee", Permissions = "Read,Write,Delete,Update" });
                    }
                    await _userManager.AddToRoleAsync(admin, "Employee");

                    var user = await _userManager.FindByNameAsync(admin.Email);

                    if (user != null)
                    {
                            var tasks = new List<GENERALTASK>
                            {
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Personal Information",
                                Description = "Please Add Your Personal Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Emergency Contacts Information",
                                Description = "Please Add Your Emergency Contacts Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Dependents Information",
                                Description = "Please Add Your Dependents Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Education Information",
                                Description = "Please Add Your Education Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Professional License Information",
                                Description = "Please Add Your Professional License Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Job Experience Information",
                                Description = "Please Add Your Job History  ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Certifications Information",
                                Description = "Please Add Your Certifications Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = company.CompanyId
                            },
                            };
                            await _generalTask_Service.AddRange(tasks);
                            await _generalTask_Service.CompleteAync();
                        
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Your Account Created Successfully";
                        notification.UserId       = user.Id;
                        notification.WorkflowStep = "Registration";
                        notification.Timestamp    = DateTime.Now;
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                        return "OK";
                    }
                    var err= result.Errors.Select(x => x.Description).FirstOrDefault();
                    return err; 
                  } 
               
                }
              return "Company Detail Already Exists";
             }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> RegisterUsers(AddUsersModel adminDto)
        {
            try
            {
                var tenantId             = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                var UserId               = _httpContextAccessor.HttpContext?.Items["CurrentUserId"]?.ToString();
                ApplicationUser userf    = await _userManager.FindByIdAsync(UserId);
                var admin                = new ApplicationUser();
                admin.FirstName          = adminDto.FirstName;
                admin.LastName           = adminDto.LastName;
                admin.MiddleName         = adminDto.MiddleName;
                admin.Email              = adminDto.Email; ;
                admin.UserName           = adminDto.Email;
                admin.defaultPassword    = adminDto.Password;
                admin.isAdmin            = adminDto.isAdmin;
                admin.isEmployee         = adminDto.isEmployee;
                admin.EmployeeId         = await getEmployeeId() + 1;
                admin.TenantId           = tenantId;
                admin.CompanyName        = userf.CompanyName;
                admin.CompanyDesignation = adminDto.Designation;
                admin.image              = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/images.jfif?alt=media&token=09284390-5fd1-40f7-b91c-feabadf143a9";
                var result               = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(adminDto.role);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = adminDto.role, Permissions = "Read,Write,Update,Delete" });
                    }

                    await _userManager.AddToRoleAsync(admin, adminDto.role);
                    var roles = await _roleManager.FindByNameAsync(adminDto.role);
                    foreach (var permission in roles.Permissions.Split(','))
                    {
                        await _userManager.AddClaimAsync(admin, new Claim("Permission", permission));
                    }

                    var user = await _userManager.FindByEmailAsync(admin.Email);

                    if (user != null)
                    {

                        if (adminDto.role == "Employee")
                        {
                            var tasks = new List<GENERALTASK>
                            {
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Personal Information",
                                Description = "Please Add Your Personal Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Emergency Contacts Information",
                                Description = "Please Add Your Emergency Contacts Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Dependents Information",
                                Description = "Please Add Your Dependents Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Education Information",
                                Description = "Please Add Your Education Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Professional License Information",
                                Description = "Please Add Your Professional License Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Job Experience Information",
                                Description = "Please Add Your Job History  ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                                new GENERALTASK
                            {
                                Id          = Guid.NewGuid(),
                                Title       = "Certifications Information",
                                Description = "Please Add Your Certifications Information ",
                                StartDate   = DateTime.Now,
                                DueDate     = DateTime.Now.AddDays(7),
                                Type        = "Task",
                                Progress    = "Pending",
                                UserId      = user.Id,
                                TenantId    = tenantId
                            },
                            };
                            await _generalTask_Service.AddRange(tasks);
                            await _generalTask_Service.CompleteAync();
                        }
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Your Account Created Successfully";
                        notification.UserId       = user.Id;
                        notification.WorkflowStep = "Registration";
                        notification.Timestamp    = DateTime.Now;
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                    }

                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> RegisterHospital(RegisterUserModel adminDto)
        {
            try
            {
                var admin       = new ApplicationUser();
                admin.FirstName = adminDto.FirstName;
                admin.LastName  = adminDto.LastName;
                admin.Email     = adminDto.Email; ;
                admin.UserName  = adminDto.Email;
                admin.EmployeeId = await getEmployeeId() + 1;
                var result      = await _userManager.CreateAsync(admin, adminDto.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("Hospital");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new CustomRole { Name = "Hospital", Permissions = "Read,Write" });
                    }
                    await _userManager.AddToRoleAsync(admin, "Hospital");
                }

                return result.Errors;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<dynamic> FindById(string uid)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(uid);
                return user;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> UpdateUser(UpdateUserModel user)
        {
            try
            {
                ApplicationUser user1 = await _userManager.FindByEmailAsync(user.Email);
                if (user1 == null)
                {
                    var message = "No User Found Against Email " + user.Email;
                    return JsonSerializer.Serialize(message);
                }

                user1.FirstName  = user.FirstName;
                user1.LastName   = user.LastName;
                user1.MiddleName = user.MiddleName;
                user1.image      = user.Image;
                var result = await _userManager.UpdateAsync(user1);
                if (result.Succeeded == true)
                {
                    var notification     = new NOTIFICATIONS();
                    notification.IsRead  = false;
                    notification.Message = "Your Account Detail Updated Successfully";
                    notification.UserId  = user1.Id;
                    notification.Timestamp = DateTime.Now;
                    notification.WorkflowStep = "Account Setting";
                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();
                    var message       = "User neccessary Details Updated Successfully";
                    return JsonSerializer.Serialize(message);
                }
                return user;
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> FindbyEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<dynamic> UpdatePassord(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    var notification          = new NOTIFICATIONS();
                    notification.IsRead       = false;
                    notification.Message      = "Your Password Detail Updated Successfully";
                    notification.UserId       = user.Id;
                    notification.Timestamp    = DateTime.Now;
                    notification.WorkflowStep = "Password Update";
                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();
                    return "OK Password updated successfully";
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).FirstOrDefault();
                }
            }
            return "User not Found";
        }
        public async Task<dynamic> ComfirmEmail(string email, string otp)
        {
            try
            {
                var currentTime = DateTime.Now;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not Found";
                }
                dynamic retVlaue = await VerifyOTP(email, otp);
                if (retVlaue is Guid)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    string subject      = "Email Confirmed Successfully";
                    string salutation   = user.FirstName + " " + user.LastName;
                    string messageBody  = CreateConfirmedEmailBody(salutation);
                    string emailMessage = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmail1Async(user.Email, subject, emailMessage);
                    var res             = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        var notification       = new NOTIFICATIONS();
                        notification.IsRead    = false;
                        notification.Message   = "Your Emai Confirmed Successfully";
                        notification.UserId    = user.Id;
                        notification.Timestamp = DateTime.Now;
                        notification.WorkflowStep = "Email Confirmation";
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                        return "Your Email Confirmed Successfully";
                    }

                    return res;
                }
                return retVlaue;



            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> SendComfirmEmail(string email)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not found";
                }
                var otp            = GenerateRandomOTP();
                var expirationTime = DateTime.Now.AddMinutes(15);
                var resetRequest   = new PasswordResetDomain
                {
                    Email          = user.Email,
                    ExpireTime     = expirationTime,
                    OTP            = otp
                };

                string retValue = await StorePasswordResetRequest(resetRequest);
                if (!retValue.StartsWith("OK"))
                {
                    return retValue;
                }
                string subject      = "Verify Email";
                string salutation   =  user.FirstName + " " + user.LastName;
                string emailMessage = CreateConfirmEmailBody(otp, salutation);
                await _emailService.SendEmail1Async(user.Email, subject, emailMessage, true);
                var notification = new NOTIFICATIONS();
                notification.IsRead = false;
                notification.Message = "Email Confirmation sent Successfully";
                notification.UserId = user.Id;
                notification.Timestamp = DateTime.Now;
                notification.WorkflowStep = "Email Confirmation";
                await _notifications_Service.InsertAsync(notification);
                await _notifications_Service.CompleteAync();
                return "Email has been sent to You for Verification";
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }
        public async Task<dynamic> VerifyOTP(string email, string otp)
        {
            var currentTime = DateTime.Now;
            IList<PasswordResetDomain> otplist = (IList<PasswordResetDomain>)await _passwordResetService.GetAll();
            if (otplist.Count == 0)
            {
                return "You did not have generated OTP";
            }
            PasswordResetDomain otpf = otplist.Where(x => x.Email == email).OrderBy(x => x.ExpireTime).LastOrDefault();
            if (currentTime <= otpf.ExpireTime && otpf.OTP == otp)
            {
                return otpf.Id;
            }

            return "OTP expired generate new ";
        }
        private async Task<string> StorePasswordResetRequest(PasswordResetDomain resetRequest)
        {

            try
            {

                await _passwordResetService.InsertAsync(resetRequest);
                await _passwordResetService.CompleteAync();
                var message = "OK";
                return message;

            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }



        }
        private static string      GenerateRandomOTP()
        {
            const string validChars = "0123456789";
            int leng = 8;
            var random = new Random();
            var otp = new string(Enumerable.Repeat(validChars, leng)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return otp;
        }
        private async Task<string> GenerateToken(ApplicationUser user,int tenantId,string CompanyStatus,string ProfileStatus,bool payment)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

            // Fetch the first role's permissions (you may adjust this based on your logic)
            var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault());
            var permissions = role?.Permissions;

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var permissionClaims = !string.IsNullOrEmpty(permissions)
                                   ? new List<Claim> { new Claim("Permission", permissions) }
                                   : new List<Claim>();

            var userClaims = await _userManager.GetClaimsAsync(user);
            bool TwoFactorEnabled;
            if (user.TwoFactorEnabled)
            {
                TwoFactorEnabled = true;
            }
            else
            {
                TwoFactorEnabled = false;
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("isAdmin", user.isAdmin ? "true" : "false"),
                new Claim("isEmployee", user.isEmployee ? "true" : "false"),
                new Claim("EmailConfirmed", user.EmailConfirmed ? "true" : "false"),
                new Claim("TwoFactorEnabled", TwoFactorEnabled ? "true" : "false"),
                new Claim("payment", payment ? "true" : "false"),
                new Claim("TenantId", tenantId.ToString()),
                new Claim("CompanyStatus", CompanyStatus),
                new Claim("ProfileStatus", ProfileStatus)
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #region Role Management
        public async Task<IEnumerable<IdentityError>> CreateRole(string roleName,string Permissions)
        {
            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new CustomRole { Name = roleName, Permissions = Permissions});
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "Role already exists." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> UpdateRoleAndPermissions(string Id,string roleName, string permissions)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Id);
                if (role == null)
                {
                    return new List<IdentityError> { new IdentityError { Description = "Role not found" } };
                }
                role.Permissions    = permissions;
                role.NormalizedName = roleName.ToUpper();
                role.Name           = roleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Enumerable.Empty<IdentityError>(); 
                }
                else
                {
                    return result.Errors; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable<IdentityError>> UpdateUserRole(string userId, string newRole)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Remove existing roles
                    var existingRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, existingRoles);

                    // Add the new role
                    await _userManager.AddToRoleAsync(user, newRole);

                    return null; // Success
                }

                return new List<IdentityError> { new IdentityError { Description = "User not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<string> UpdateRole()
        {
            try
            {
                var existingRole = await _roleManager.FindByNameAsync("Candidate");
                existingRole.Permissions = "Read";
                await _roleManager.UpdateAsync(existingRole);
                return "OK";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable<IdentityError>> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var result = await _roleManager.DeleteAsync(role);
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "Role not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable<IdentityError>> DeleteUser(string id)
        {
            try
            {
                var role = await _userManager.FindByIdAsync(id);
                if (role != null)
                {
                    var result = await _userManager.DeleteAsync(role);
                    return result.Errors;
                }

                return new List<IdentityError> { new IdentityError { Description = "User not found." } };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }
        public async Task<IEnumerable> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<IEnumerable> GetAllUsersWithRoles()
        {
            try
            {
                var tenantId    = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                if (tenantId!=4)
                {
                    var users       = await _userManager.Users.Where(x => x.TenantId == tenantId).ToListAsync();
                    var userResults = users.Select(user =>
                    {
                        var roles = _userManager.GetRolesAsync(user);

                        return new
                        {
                            user.Id,
                            user.FirstName,
                            user.MiddleName,
                            user.LastName,
                            user.Email,
                            Roles = roles.Result.FirstOrDefault(),
                            user.image,
                            user.defaultPassword,
                            user.CompanyName,
                            user.EmployeeId,
                            user.CompanyDesignation
                        };
                    }).ToList();

                    return userResults.ToList();
                }
                else
                {
                    var users = await _userManager.Users.ToListAsync();
                    var userResults = users.Select(user =>
                    {
                        var roles = _userManager.GetRolesAsync(user);

                        return new
                        {
                            user.Id,
                            user.FirstName,
                            user.MiddleName,
                            user.LastName,
                            user.Email,
                            Roles = roles.Result.FirstOrDefault(),
                            user.image,
                            user.defaultPassword,
                            user.CompanyName,
                            user.EmployeeId,
                            user.CompanyDesignation
                        };
                    }).ToList();

                    return userResults.ToList();
                }
               
            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }


        //Get All Users in the 
        public async Task<IEnumerable> GetAll()
        {
            try
            {
                var users       = await _userManager.Users.ToListAsync();
                var userResults = users.Select(user =>
                {
                    var roles = _userManager.GetRolesAsync(user);
                    return new AllUsersModel
                    {
                      Id         =  user.Id,
                      FirstName  =  user.FirstName,
                      MiddleName =  user.MiddleName,
                      LastName   =  user.LastName,
                      Email      =  user.Email,
                      Roles      =  roles.Result.FirstOrDefault(),
                      Image      =  user.image,

                    };
                }).ToList();
                return userResults.ToList();

            }
            catch (Exception ex)
            {
                return ex.Message + ex.InnerException?.Message;
            }
        }

        public async Task<AllUsersModel> GetByIduser(string uid)
        {
            try
            {
                var user    = await _userManager.FindByIdAsync(uid);
                var roles   = _userManager.GetRolesAsync(user);

                var myuser = new AllUsersModel
                {
                    Id         = user.Id,
                    FirstName  = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName   = user.LastName,
                    Email      = user.Email,
                    Roles      = roles.Result.FirstOrDefault(),
                    Image      = user.image,
                };




                return myuser;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<dynamic> UpdateCRMUser(AddUsersModel user)
        {
            try
            {
                ApplicationUser user1 = await _userManager.FindByEmailAsync(user.Email);

                if (user1 == null)
                {
                    var errorMessage = "No User Found Against Email " + user.Email;
                    return errorMessage;
                }

                // Update user details
                user1.FirstName          = user.FirstName;
                user1.LastName           = user.LastName;
                user1.MiddleName         = user.MiddleName;
                user1.Email              = user.Email;
                user1.isEmployee         = user.isEmployee;
                user1.isAdmin            = user1.isAdmin;
                user1.CompanyDesignation = user.Designation;
                // Update user role
                var existingRoles = await _userManager.GetRolesAsync(user1);
                var roleToRemove  = existingRoles.FirstOrDefault();
                if (roleToRemove != null)
                {
                    var removeRoleResult = await _userManager.RemoveFromRoleAsync(user1, roleToRemove);
                    if (!removeRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to remove existing role.";
                        return errorMessage;
                    }

                    var addRoleResult = await _userManager.AddToRoleAsync(user1, user.role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to add the new role.";
                        return errorMessage;
                    }
                }
                else
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user1, user.role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessage = "Failed to add the new role.";
                        return errorMessage;
                    }
                }


                // Update other details
                var updateResult = await _userManager.UpdateAsync(user1);
                if (updateResult.Succeeded)
                {
                    var notification = new NOTIFICATIONS
                    {
                        IsRead = false,
                        Message = "Your Account Detail Updated Successfully",
                        UserId = user1.Id,
                        Timestamp = DateTime.Now,
                        WorkflowStep = "Account Setting"
                    };

                    await _notifications_Service.InsertAsync(notification);
                    await _notifications_Service.CompleteAync();

                    var successMessage = "User's necessary Details Updated Successfully";
                    return successMessage;
                }

                var failureMessage = "Failed to update user details.";
                return failureMessage;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> RegisterTenant(TenantRegisterModel model)
        {
            try
            {
                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x => x.CompanyName.ToLower() == model.CompanyName.ToLower());
                if (list.Count == 0)
                {
                     var admin                = new ApplicationUser();
                     admin.FirstName          = model.FirstName;
                     admin.LastName           = model.LastName;
                     admin.MiddleName         = model.MiddleName;
                     admin.Email              = model.Email;
                     admin.UserName           = model.Email;
                     admin.CompanyName        = model.CompanyName;
                     admin.EmployeeId         =  await getEmployeeId() + 1; ;
                     admin.CompanyDesignation = "Onwer";
                     admin.image              = "https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/images.jfif?alt=media&token=09284390-5fd1-40f7-b91c-feabadf143a9";
                     var result               = await _userManager.CreateAsync(admin, model.Password);
                     if (result.Succeeded)
                     {
                         var roleExists = await _roleManager.RoleExistsAsync("SuperUser");
                         if (!roleExists)
                         {
                             await _roleManager.CreateAsync(new CustomRole { Name = "SuperUser", Permissions = "Read,Write,Delete,Update" });
                         }
                     
                        await _userManager.AddToRoleAsync(admin, "SuperUser");
                        var user = await _userManager.FindByEmailAsync(admin.Email);
                        if (user != null)
                        {
                            
                            var company      = new PelicanHRMTenant()
                            {
                                CompanyName   = model.CompanyName,
                                CompanyStatus = "Active",
                                ProfileStatus = "InCompleted"

                            };
                            
                            await _tenants_Service.InsertAsync(company);
                            await _tenants_Service.CompleteAync();

                            var notification          = new NOTIFICATIONS();
                            notification.IsRead       = false;
                            notification.Message      = "Your Account Created Successfully";
                            notification.UserId       = user.Id;
                            notification.WorkflowStep = "Registration";
                            notification.Timestamp    = DateTime.Now;
                            await _notifications_Service.InsertAsync(notification);
                            await _notifications_Service.CompleteAync();

                            IList<PelicanHRMTenant> list1 = await _tenants_Service.Find(x => x.CompanyName.ToLower() == model.CompanyName.ToLower());

                            if (list1.Count != 0)
                            {
                                PelicanHRMTenant tenant = list1.FirstOrDefault();
                                user.CompanyName        = tenant.CompanyName;
                                user.TenantId           = tenant.CompanyId;

                                await _userManager.UpdateAsync(user);
                            }
                        }

                        return "OK";
                    }
                    var err= result.Errors.Select(x => x.Description).FirstOrDefault();
                    return err; 
                }
                else
                {
                    return "Company Detail Already Exists";
                }
                

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private async Task<int> getEmployeeId()
        {
            int maxEmployeeId = await _userManager.Users.Select(x => (int?)x.EmployeeId).MaxAsync() ?? 0;
            return maxEmployeeId;
        }

        public async Task<string> GetCompanyByName(string CompanyName)
        {
            try
            {
                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x=>x.CompanyName.Trim().ToLower()==CompanyName.Trim().ToLower());
                if (list.Count==0)
                {
                    return "OK";
                }
                else
                {
                    return list.Select(x => x.CompanyName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<string> UpdateTenant(TenantUpdate model)
        {
            try
            {
                var tenantId = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x => x.CompanyId==tenantId);
                if (list.Count != 0)
                {
                    PelicanHRMTenant tenant  = list.FirstOrDefault();
                    if(tenant!= null) 
                    {
                       tenant.EIN                  = model.EIN;
                       tenant.AddressLine1         = model.AddressLine1;
                       tenant.AddressLine2         = model.AddressLine2;
                       tenant.isPhysicalAddress    = model.isPhysicalAddress;
                       tenant.isMailingAddress     = model.isMailingAddress;
                       tenant.BussinessType        = model.BussinessType; 
                       tenant.City                 = model.City;
                       tenant.State                = model.State;
                       tenant.ProfileStatus        = "Completed";
                       tenant.LegalName            = model.LegalName;
                       tenant.noDomesticContractor = model.noDomesticContractor;
                       tenant.noDomesticEmployee   = model.noDomesticEmployee;
                       tenant.FillingFormIRS       = model.FillingFormIRS;
                       tenant.Industry             = model.Industry;
                       tenant.noInterContractor    = model.noInterContractor;
                       tenant.Country              = model.Country;
                       tenant.noInterEmployee      = model.noInterEmployee;
                       tenant.PhoneNumber          = model.PhoneNumber;
                       tenant.whosisCompany        = model.whosisCompany;
                       tenant.State                = model.State;
                       tenant.CreatedAt            = DateTime.Now;
                       tenant.ZipCode              = model.ZipCode;
                        _context.Entry(tenant).Property("CompanyId").IsModified = false;
                        _context.Update(tenant);
                        _context.Entry(tenant).Property("CompanyId").IsModified = false;
                        _context.SaveChanges();

                    }

                    
                    return "OK";
                }
                else
                {
                    return "Error in Registering Company";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }


        public async Task<TenantUpdate> GetUpdateTenant()
        {
            try
            {
                var tenantId                 = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["CurrentTenant"]);
                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x => x.CompanyId==tenantId);
                if (list.Count != 0)
                {
                    PelicanHRMTenant tenant    = list.FirstOrDefault();
                    TenantUpdate     Tenants   = new TenantUpdate();
                    if (tenant!= null) 
                    {
                        Tenants.EIN                   = tenant.EIN;                 
                        Tenants.AddressLine1          = tenant.AddressLine1;
                        Tenants.AddressLine2          = tenant.AddressLine2;
                        Tenants.isPhysicalAddress     = tenant.isPhysicalAddress;
                        Tenants.isMailingAddress      = tenant.isMailingAddress;
                        Tenants.BussinessType         = tenant.BussinessType;
                        Tenants.City                  = tenant.City;
                        Tenants.State                 = tenant.State;
                        Tenants.LegalName             = tenant.LegalName;
                        Tenants.noDomesticContractor  = tenant.noDomesticContractor.GetValueOrDefault();
                        Tenants.noDomesticEmployee    = tenant.noDomesticEmployee.GetValueOrDefault();
                        Tenants.FillingFormIRS        = tenant.FillingFormIRS;
                        Tenants.Industry              = tenant.Industry;
                        Tenants.noInterContractor     = tenant.noInterContractor.GetValueOrDefault();
                        Tenants.Country               = tenant.Country;
                        Tenants.noInterEmployee       = tenant.noInterEmployee.GetValueOrDefault();
                        Tenants.PhoneNumber           = tenant.PhoneNumber;
                        Tenants.whosisCompany         = tenant.whosisCompany;
                        Tenants.State                 = tenant.State;
                        Tenants.ZipCode               = tenant.ZipCode;              
                       

                    }


                    return Tenants;
                }
                else

                {
                    TenantUpdate Tenants1 = new TenantUpdate();
                    return Tenants1;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        public async Task<string> UpdateTenantStatus(TenantBlock model)
        {
            try
            {
               
                IList<PelicanHRMTenant> list = await _tenants_Service.Find(x => x.CompanyId==model.Id);
                if (list.Count != 0)
                {
                    PelicanHRMTenant tenant  = list.FirstOrDefault();
                    if(tenant!= null) 
                    {
                       tenant.CompanyStatus        = model.Status;
                       tenant.CreatedAt            = DateTime.Now;
                        _context.Entry(tenant).Property("CompanyId").IsModified = false;
                        _context.Update(tenant);
                        _context.Entry(tenant).Property("CompanyId").IsModified = false;
                        _context.SaveChanges();
                    }

                    
                    return "OK";
                }
                else
                {
                    return "Error in Registering Company";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + ex.InnerException?.Message);
            }
        }

        #endregion


        #region Email Templates
        private static string CreateConfirmEmailBody(string OTP, string Username)
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
                          border-bottom: #F26302 solid 5px;
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
                                          src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/BottomLogo.jpeg?alt=media&token=48c6d297-9821-4d2c-bb7f-33ea079043f7""
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
                                Verify Email
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
                                 Thank you for registering with PelicanHRM.
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
                                 Your email verification OTP
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
                                    background: linear-gradient(to right,rgb(254,0,0), rgb(235,123,49), rgb(235,123,49), rgb(235,123,49) 30%, rgb(235,123,49) 70%, rgb(255,165,0)) !important;
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
        private static string CreateConfirmedEmailBody(string Username)
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
                      background: #f9f9f9;
                      background-color: #f9f9f9;
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
                              border-bottom: #F26302 solid 5px;
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
                                              src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/BottomLogo.jpeg?alt=media&token=48c6d297-9821-4d2c-bb7f-33ea079043f7""
                                              style=""
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
                                    Email is confirmed successfully
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
                                     Your email is confirmed successfully.
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
                                     Please Login in to PelicanHRM
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
                                            padding: 15px 25px;
                                          ""
                                          valign=""middle""
                                        >
                                       <img
                                          height=""auto""
                                          src=""https://firebasestorage.googleapis.com/v0/b/images-107c9.appspot.com/o/email.png?alt=media&token=af53911e-21cb-4445-ab72-bbb98e257507""
                                          style=""
                                            border: 0;
                                            display: block;
                                            outline: none;
                                            text-decoration: none;
                                            width: 100%;
                                          ""
                                          width=""64""
                                        />
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

        public async Task<dynamic> EnableTwoFactorAuthEmail(string email, string otp)
        {
            try
            {
                var currentTime      = DateTime.Now;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not Found";
                }
                dynamic retVlaue = await VerifyOTP(email, otp);
                if (retVlaue is Guid)
                {
                    user.TwoFactorEnabled = true;
                    await _userManager.UpdateAsync(user);
                    string subject        = "Email OTP Successfully";
                    string salutation     = user.FirstName + " " + user.LastName;
                    string messageBody    = CreateConfirmedEmailBody(salutation);
                    string emailMessage   = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmail1Async(user.Email, subject, emailMessage);
                    var res = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Two factor authentication verification success";
                        notification.UserId       = user.Id;
                        notification.Timestamp    = DateTime.Now;
                        notification.WorkflowStep = "Email Confirmation";
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                        return "Two factor authentication verification success";
                    }

                    return res;
                }
                return retVlaue;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> DisableTwoFactorAuthEmail(string email, string otp)
        {
            try
            {
                var currentTime      = DateTime.Now;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not Found";
                }
                dynamic retVlaue = await VerifyOTP(email, otp);
                if (retVlaue is Guid)
                {
                    user.TwoFactorEnabled = false;
                    await _userManager.UpdateAsync(user);
                    string subject        = "Email OTP Successfully";
                    string salutation     = user.FirstName + " " + user.LastName;
                    string messageBody    = CreateConfirmedEmailBody(salutation);
                    string emailMessage   = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmail1Async(user.Email, subject, emailMessage);
                    var res = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        var notification          = new NOTIFICATIONS();
                        notification.IsRead       = false;
                        notification.Message      = "Two factor authentication verification success";
                        notification.UserId       = user.Id;
                        notification.Timestamp    = DateTime.Now;
                        notification.WorkflowStep = "Email Confirmation";
                        await _notifications_Service.InsertAsync(notification);
                        await _notifications_Service.CompleteAync();
                        return "Two factor authentication verification success";
                    }

                    return res;
                }
                return retVlaue;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<dynamic> VerifyTwoFactorAuthEmail(string email, string otp)
        {
            try
            {
                var currentTime      = DateTime.Now;
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "User not Found";
                }
                dynamic retVlaue = await VerifyOTP(email, otp);
                if (retVlaue is Guid)
                {
                    var res = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        return "Your session started now";
                    }
                    else
                    {
                        return "Something Went Wrong";
                    }
                }
                return retVlaue;
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }

        public async Task<IEnumerable> GetTenants()
        {
            try
            {
                IList<PelicanHRMTenant> tenants = (IList<PelicanHRMTenant>)await _tenants_Service.GetAll();
                IList<ApplicationUser> userList = await _userManager.Users.ToListAsync();
                var groupedUsersByCompany = userList
                    .GroupBy(user => user.TenantId)
                    .ToDictionary(group => group.Key, group => group.Select(user => new UserWithRolesModelForTenants
                    {
                        Id                 = user.Id,
                        Name               = user.FirstName + " " + user.LastName,
                        Email              = user.Email,
                        Role               = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Image              = user.image,
                        CompanyName        = user.CompanyName,
                        TenantId           = user.TenantId,
                        CompanyDesignation = user.CompanyDesignation
                    }).ToList());

                // Create the list of UserWithCompanyUsers objects
                var userWithCompanyUsersList = new List<UserWithRolesModelForTenants>();

                // Iterate through each user
                foreach (var user in userList)
                {
                    // Get the list of users in the same company (TenantId)
                    var companyUsers = groupedUsersByCompany[user.TenantId];

                    // Create a UserWithCompanyUsers object
                    var userWithCompanyUsers = new UserWithRolesModelForTenants
                    {
                        Id                  = user.Id,
                        Name                = user.FirstName + " " + user.LastName,
                        Email               = user.Email,
                        Role                =  _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Image               = user.image,
                        CompanyName         = user.CompanyName,
                        TenantId            = user.TenantId,
                        CompanyDesignation  = user.CompanyDesignation,
                        List                = companyUsers // Add the list of users in the same company
                    };

                    // Add the user with company users to the list
                    userWithCompanyUsersList.Add(userWithCompanyUsers);
                }
                IList<UserWithRolesModelForTenants> users = userWithCompanyUsersList.Where(x => x.Role == "SuperUser").ToList();
                var finalist=users.Join
                    (
                     tenants,
                     (u) => new { u.TenantId },
                     (t) => new { TenantId = t.CompanyId },
                     (u, t) => new { u, t }
                    )
                    .Select(user => new
                    {
                        Id                   = user.u.Id,
                        Name                 = user.u.Name,
                        Email                = user.u.Email,
                        Role                 = user.u.Role,
                        Image                = user.u.Image,
                        CompanyName          = user.u.CompanyName,
                        TenantId             = user.u.TenantId,
                        CompanyDesignation   = user.u.CompanyDesignation,
                        CompanyStatus        = user.t.CompanyStatus,
                        Profilestatus        = user.t.ProfileStatus,
                        PhoneNumber          = user.t.PhoneNumber,
                        List                 = user.u.List
                    })
                    .ToList();

                return finalist;
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }



        public async Task<dynamic> GetTenantsbyId(string OnwerId)
        {
            try
            {
                IList<PelicanHRMTenant> tenants = (IList<PelicanHRMTenant>)await _tenants_Service.GetAll();
                IList<ApplicationUser> userList = await _userManager.Users.ToListAsync();
                var groupedUsersByCompany = userList
                    .GroupBy(user => user.TenantId)
                    .ToDictionary(group => group.Key, group => group.Select(user => new UserWithRolesModelForTenants
                    {
                        Id                 = user.Id,
                        Name               = user.FirstName + " " + user.LastName,
                        Email              = user.Email,
                        Role               = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Image              = user.image,
                        CompanyName        = user.CompanyName,
                        TenantId           = user.TenantId,
                        CompanyDesignation = user.CompanyDesignation
                    }).ToList());

                // Create the list of UserWithCompanyUsers objects
                var userWithCompanyUsersList = new List<UserWithRolesModelForTenants>();

                // Iterate through each user
                foreach (var user in userList)
                {
                    // Get the list of users in the same company (TenantId)
                    var companyUsers = groupedUsersByCompany[user.TenantId];

                    // Create a UserWithCompanyUsers object
                    var userWithCompanyUsers = new UserWithRolesModelForTenants
                    {
                        Id                  = user.Id,
                        Name                = user.FirstName + " " + user.LastName,
                        Email               = user.Email,
                        Role                =  _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                        Image               = user.image,
                        CompanyName         = user.CompanyName,
                        TenantId            = user.TenantId,
                        CompanyDesignation  = user.CompanyDesignation,
                        List                = companyUsers // Add the list of users in the same company
                    };

                    // Add the user with company users to the list
                    userWithCompanyUsersList.Add(userWithCompanyUsers);
                }
                IList<UserWithRolesModelForTenants> users = userWithCompanyUsersList.Where(x => x.Role == "SuperUser").ToList();
                var finalist=users.Join
                    (
                     tenants,
                     (u) => new { u.TenantId },
                     (t) => new { TenantId = t.CompanyId },
                     (u, t) => new { u, t }
                    )
                    .Where(x=>x.u.Id== OnwerId || x.u.Email==OnwerId)
                    .Select(user => new
                    {
                        Id                   = user.u.Id,
                        Name                 = user.u.Name,
                        Email                = user.u.Email,
                        Role                 = user.u.Role,
                        Image                = user.u.Image,
                        CompanyName          = user.u.CompanyName,
                        TenantId             = user.u.TenantId,
                        CompanyDesignation   = user.u.CompanyDesignation,
                        CompanyStatus        = user.t.CompanyStatus,
                        Profilestatus        = user.t.ProfileStatus,
                        PhoneNumber          = user.t.PhoneNumber,
                         AddressLine1        = user.t.AddressLine1,
                        AddressLine2         = user.t.AddressLine2,
                        City                 = user.t.City,
                        State                = user.t.State,
                        Country              = user.t.Country,
                        ZipCode              = user.t.ZipCode,
                        Longitude            = user.t.Longitude,
                        Latitude             = user.t.Latitude,
                        IsMailingAddress     = user.t.isMailingAddress,
                        IsPhysicalAddress    = user.t.isPhysicalAddress,
                        WhosisCompany        = user.t.whosisCompany,
                        NoDomesticEmployee   = user.t.noDomesticEmployee,
                        NoInterEmployee      = user.t.noInterEmployee,
                        NoDomesticContractor = user.t.noDomesticContractor,
                        NoInterContractor    = user.t.noInterContractor,
                        EIN                  = user.t.EIN,
                        BusinessType         = user.t.BussinessType,
                        LegalName            = user.t.LegalName,
                        FillingFormIRS       = user.t.FillingFormIRS,
                        Industry             = user.t.Industry,
                        CreatedAt            = user.t.CreatedAt.ToString("MM/dd/yyyy"),
                        List                 = user.u.List
                    })
                    .LastOrDefault();

                return finalist;
            }
            catch (Exception ex)
            {

                return ex.Message + ex.InnerException?.Message;
            }
        }

        #endregion
    }



}
