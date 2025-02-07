using Server.Models;
using System.Collections;
using Server.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Server.Services
{
    public interface IAuth_Manager_Service
    {
        Task<AuthResponseModel> Login(UserLoginModel loginDto);
        Task<string> Register(UserRegisterModel loginDto);
        Task<IEnumerable<IdentityError>> RegisterAdmin(RegisterUserModel adminDto);
        Task<string> RegisterCandidates(RegisterUserModel adminDto);
        Task<IEnumerable<IdentityError>> RegisterUsers(AddUsersModel adminDto);
        Task<IEnumerable<IdentityError>> RegisterHospital(RegisterUserModel adminDto);
        Task<dynamic> FindById(string uid);
        Task<dynamic> FindById(int uid);
        Task<dynamic> UpdateUser(UpdateUserModel user);
        Task<dynamic> UpdateCRMUser(AddUsersModel user);
        Task<dynamic> FindbyEmail(string email);
        Task<dynamic> UpdatePassord(string email, string password);
        Task<dynamic> ComfirmEmail(string email, string otp);
        Task<dynamic> EnableTwoFactorAuthEmail(string email, string otp);
        Task<dynamic> VerifyTwoFactorAuthEmail(string email, string otp);
        Task<dynamic> SendComfirmEmail(string email);
        Task<dynamic> VerifyOTP(string email, string otp);
        Task<IEnumerable<IdentityError>> CreateRole(string roleName,string Permissions);
        Task<IEnumerable<IdentityError>> UpdateUserRole(string userId, string newRole);
        Task<IEnumerable<IdentityError>> DeleteRole(string id);
        Task<IEnumerable<IdentityError>> DeleteUser(string id);
        Task<IEnumerable> GetAllRoles();
        Task<IEnumerable> GetAllUsersWithRoles();
        Task<IEnumerable> GetAll();
        Task<AllUsersModel> GetByIduser(string uid);
        Task<string> UpdateRole();
        Task<IEnumerable<IdentityError>> UpdateRoleAndPermissions(string Id, string roleName, string permissions);
        Task<string> RegisterTenant(TenantRegisterModel model);
        Task<string> UpdateTenant(TenantUpdate model);
        Task<string> UpdateTenantStatus(TenantBlock tenantBlock);
        Task<string> GetCompanyByName(string CompanyName);
        Task<dynamic> DisableTwoFactorAuthEmail(string email, string otp);
        Task<dynamic> GetTenantsbyId(string OnwerId);
        Task<IEnumerable> GetTenants();
        Task<TenantUpdate> GetUpdateTenant();
        Task<string> SendResetPassword(string Id);
        Task<dynamic> GetTenantsDetailbyId(string OnwerId);
        Task<IEnumerable<IdentityError>> RegisterSuperAdmin(RegisterUserModel adminDto);
        Task<IList<AllUsersModel>> GetUsersListByIds(IList<string> Ids);
        Task<IEnumerable> GetAllUsersWithRoles(int tenantId);

    }
}
