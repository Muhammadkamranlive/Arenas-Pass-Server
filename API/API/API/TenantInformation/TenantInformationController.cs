using Server.Domain;
using Server.Services;
using Server.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace API.API.TenantInformation
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantInformationController : ControllerBase
    {
        #region Constructor
        private readonly INotifications_Service       _notifyService;
        private readonly IAuthManager                 _authManager;
        private readonly ITenants_Service             _tenantService;
        private readonly IAccount_Balance_Service     _accountBalanceService;
        private readonly IAccount_Transaction_Service _accountTransactionService;
        private readonly IGift_Card_Service           _giftCardService;
        private readonly ITenant_License_Keys_Service _License_Keys_Service;
        private readonly ITenant_Api_Hits_Service     _tenant_Api_Hits_Service;
        private readonly ITenant_Key_History_Service  _tenant_Key_History_Service;
        private readonly ICaseManagment_Service       _caseManageService;
        private readonly ICaseComments_Service        _casecommentService;
        private readonly ILog_Service                 _log_Servie;
        private readonly IWallet_Pass_Service        _Pass_Service;
        public TenantInformationController
        (
            INotifications_Service       no_Service, 
            IAuthManager                 authManager,
            ITenants_Service             tenant,
            IAccount_Balance_Service     account_Balance,
            IAccount_Transaction_Service accountxn,
            IGift_Card_Service           gift_Card, 
            ITenant_License_Keys_Service tenant_Keys,
            ITenant_Api_Hits_Service     tenant_Api_Hits,
            ITenant_Key_History_Service  tenant_Key_History,
            ICaseManagment_Service       caseManagment,
            ICaseComments_Service        caseComments,
            ILog_Service                 log,
            IWallet_Pass_Service         wallet_Pass
        )
        {
            _authManager                = authManager;
            _notifyService              = no_Service;
            _tenantService              = tenant;
            _accountBalanceService      = account_Balance;
            _accountTransactionService  = accountxn;
            _giftCardService            = gift_Card;
            _License_Keys_Service       = tenant_Keys;
            _tenant_Api_Hits_Service    = tenant_Api_Hits;
            _tenant_Key_History_Service = tenant_Key_History;
            _casecommentService         = caseComments;
            _caseManageService          = caseManagment;
            _log_Servie                 = log;
            _Pass_Service               = wallet_Pass;

        }

        #endregion

        [HttpGet]
        [Route("GetUserWithRoles")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> GetUserWithRoles(int TenantId)
        {
            var users = await _authManager.GetAllUsersWithRoles(TenantId);
            return Ok(users);
        }


        [HttpGet]
        [Route("GetDevTickets")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> GetDevTickets(int TenantId)
        {
            var users = await _caseManageService.Find(x=>x.TenantId==TenantId);
            return Ok(users);
        }


        [HttpGet]
        [Route("getById")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> getById(int uid)
        {
            try
            {
                var authResponse = await _authManager.FindById(uid);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Find User By User Id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("FindUserById")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> FindUserById(string uid)
        {
            try
            {
                var authResponse = await _authManager.FindById(uid);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetTenantInfo")]
        [CustomAuthorize("Read")]
        public async Task<ActionResult> GetTenantInfo(int uid)
        {
            try
            {
                var authResponse = await _tenantService.FindOne(x=>x.CompanyId==uid);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("TenantLogs")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> TenantLogs(int TenantId)
        {
            try
            {
                IList<AdminLogs> per = await _log_Servie.Find(x=>x.TenantId==TenantId);
                per = per.OrderBy(x => x.Timestamp).ToList();
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("Notifications")]
        [CustomAuthorize("Read")]
        public async Task<IActionResult> Notifications(int  TenantId)
        {
            try
            {
                IList<NOTIFICATIONS> per = await _notifyService.Find(x => x.TenantId == TenantId);
                per = per.OrderByDescending(x => x.Timestamp).ToList();
                return Ok(per);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message + e.InnerException?.Message);
            }
        }


        [HttpGet]
        [Route("GetApiStats")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetApiStats(int TenantId)
        {
            try
            {
                var Apis = await _tenant_Api_Hits_Service.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTenantApiKeys")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetTenantApiKeys(int TenantId)
        {
            try
            {
                var Apis = await _License_Keys_Service.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetTenantKeyRotationHistory")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> GetTenantKeyRotationHistory(int TenantId)
        {
            try
            {
                var Apis = await _tenant_Key_History_Service.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetWalletPasses")]
        public async Task<dynamic> GetWalletPasses(int TenantId)
        {
            try
            {
                var Apis = await _giftCardService.Find(x => x.TenantId == TenantId);
                return Ok(Apis);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Redemption
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TransactionsByTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> TransactionsByTenant(int TenantId)
        {
            try
            {
                return await _accountTransactionService.Find(x => x.Tenant_Id == TenantId.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Redemption
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OpeningBalanceByTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> OpeningBalanceByTenant(int TenantId)
        {
            try
            {
                return await _accountBalanceService.Find(x => x.Tenant_Id == TenantId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        

    }
}
