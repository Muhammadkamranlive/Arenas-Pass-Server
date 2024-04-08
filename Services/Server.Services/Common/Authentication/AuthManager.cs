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
                        paid = true;
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
                var token = await GenerateToken(user, TenantId, CompanyStatus, ProfileStatus,paid);
                return new AuthResponseModel
                {
                    Token         = token,
                    UserId        = user.Id,
                    EmailStatus   = user.EmailConfirmed,
                    CompanyStatus = CompanyStatus,
                    ProfileStatus = ProfileStatus,
                    paid          = paid,
                    Message       = "Success",
                    Name          = user.FirstName+" "+user.LastName

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
                    var notification = new NOTIFICATIONS();
                    notification.IsRead = false;
                    notification.Message = "Your Password Detail Updated Successfully";
                    notification.UserId = user.Id;
                    notification.Timestamp = DateTime.Now;
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
                    string salutation   = "Dear " + user.FirstName + " " + user.LastName;
                    string messageBody  = $"<h1>Your Email confirmed Successfully thanks to be part of Sapient Medical  </h1>";
                    string emailMessage = $"{subject}\n\n{salutation}\n\n{messageBody}";
                    await _emailService.SendEmailAsync(user.Email, subject, emailMessage);
                    var res             = await _passwordResetService.Delete(retVlaue);
                    if (res)
                    {
                        await _passwordResetService.CompleteAync();
                        var notification = new NOTIFICATIONS();
                        notification.IsRead = false;
                        notification.Message = "Your Emai Confirmed Successfully";
                        notification.UserId = user.Id;
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
                string subject = "Verify Email";
                string salutation = "Dear" + " " + user.FirstName + " " + user.LastName;
                string messageBody = $"\r\n\r\n\n\n\nThank you for registering with PelicanHRM.\r\n\r\nEmail Verification OTP\r\n\r\n\n\n\n:<h1>{otp}</h1>";

                string emailMessage = $"\r\n\r\n\n<h1>{salutation}</h1>\r\n\r\n\n\n\n{messageBody}";
                await _emailService.SendEmailAsync(user.Email, subject, emailMessage, true);
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

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("isAdmin", user.isAdmin ? "true" : "false"),
                new Claim("isEmployee", user.isEmployee ? "true" : "false"),
                new Claim("EmailConfirmed", user.EmailConfirmed ? "true" : "false"),
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
                                CompanyStatus = "Actve",
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
                       tenant.noDomesticContractor = tenant.noDomesticContractor;
                       tenant.noDomesticEmployee   = tenant.noDomesticEmployee;
                       tenant.FillingFormIRS       = tenant.FillingFormIRS;
                       tenant.Industry             = tenant.Industry;
                       tenant.noInterContractor    = tenant.noInterContractor;
                       tenant.Country              = model.Country;
                       tenant.noInterEmployee      = model.noInterEmployee;
                       tenant.PhoneNumber          = model.PhoneNumber;
                       tenant.whosisCompany        = model.whosisCompany;
                       tenant.State                = model.State;
                       tenant.CreatedAt            = DateTime.Now;
                        _tenants_Service.UpdateRecord(tenant);
                        await _tenants_Service.CompleteAync();

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
                        _tenants_Service.UpdateRecord(tenant);
                        await _tenants_Service.CompleteAync();
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
    }



}
