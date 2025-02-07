using Server.Core;
using Server.Repository;
using Microsoft.AspNetCore.Http;

namespace Server.UOW
{
    public class Unit_Of_Work_Repo : IUnit_Of_Work_Repo
    {
        private readonly ERPDb _crmContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Unit_Of_Work_Repo(ERPDb candidateCRM, IHttpContextAccessor httpContextAccessor)
        {
                _crmContext             = candidateCRM;
               _httpContextAccessor     = httpContextAccessor;
               PasswordReset_Repo       = new Password_Reset_Repo(_crmContext, _httpContextAccessor);
               Candidate_Repo           = new Candidate_Repo(_crmContext, _httpContextAccessor);
               EmergencyContacts_Repo   = new EmergencyContacts_Repo(_crmContext, _httpContextAccessor);
               CaseCommetns_Repo        = new CaseCommetns_Repo(_crmContext, _httpContextAccessor);
               Case_Repo                = new Case_Repo(_crmContext, httpContextAccessor);
               AssetManager_Repo        = new AssetManager_Repo(_crmContext, httpContextAccessor);
               Attachments_Repo         = new Attachments_Repo(_crmContext, httpContextAccessor);
               HRNotes_Repo             = new HRNotes_Repo(_crmContext, httpContextAccessor);
               Notifications_Repo       = new Notifications_Repo(_crmContext, httpContextAccessor);
               Candidate_Repo           = new Candidate_Repo(_crmContext, httpContextAccessor);
               Education_Repo           = new Education_Repo(_crmContext, httpContextAccessor);
               JobExperience_Repo       = new Job_Experience_Repo(_crmContext, httpContextAccessor);
               ProfessionalLicense_Repo = new Professional_License_Repo(_crmContext, httpContextAccessor);
               Personal_Repo            = new Personal_Repo(_crmContext, httpContextAccessor);
               zoomMeeting_Repo         = new Zoom_Meeting_Repo(_crmContext, httpContextAccessor);
               page_Repo                = new Web_Page_Repo(_crmContext, httpContextAccessor);
               blog_Repo                = new Blog_Repo(_crmContext, httpContextAccessor);
               ContactPage_             = new Contact_Page_Repo(_crmContext, httpContextAccessor);
               tenants_Repo             = new Tenants_Repo(_crmContext, httpContextAccessor);
               Designation_Repo         = new Designation_Repo(_crmContext, httpContextAccessor);
               chat_Repo                = new Chat_Repo(_crmContext, httpContextAccessor);
               Apple_Pass_Account       = new Apple_Pass_Account_Repo(_crmContext, httpContextAccessor);
               Process_Repo             = new User_Batch_Process_Repo(_crmContext, httpContextAccessor);
               Wallet_Pass              = new Wallet_Pass_Repo(_crmContext, httpContextAccessor);
               Account_Transaction      = new Account_Transaction_Repo(_crmContext,httpContextAccessor);
               Account_Balance          = new Account_Balance_Repo (_crmContext, httpContextAccessor);
               Pass_Transaction         = new Pass_Transmission_Repo(_crmContext, httpContextAccessor);
               User_Vault               = new User_Vault_Repo(_crmContext, httpContextAccessor);
               Arenas_Billing           = new Arenas_Billing_Repo(_crmContext, httpContextAccessor);

        }
        public IPassword_Reset_Repo PasswordReset_Repo             { get; private set; }
        public IContactDetails_Repo ContactDetails_Repo           { get; private set; }
        public IEmergencyContacts_Repo EmergencyContacts_Repo     { get; private set; }
        public ICaseCommetns_Repo CaseCommetns_Repo               { get; private set; }
        public ICase_Repo Case_Repo                               { get; private set; }
        public IAssetManager_Repo AssetManager_Repo               { get; private set; }
        public IAttachments_Repo Attachments_Repo                 { get; private set; }
        public IHRNotes_Repo HRNotes_Repo                         { get; private set; }
        public INotifications_Repo Notifications_Repo             { get; private set; }
        public ICandidate_Repo Candidate_Repo                     { get; private set; }
        public IEducation_Repo Education_Repo                     { get; private set; }
        public IJob_Experience_Repo JobExperience_Repo             { get; private set; }
        public IProfessional_License_Repo ProfessionalLicense_Repo { get; private set; }
        public IPersonal_Repo Personal_Repo                       { get; private set; }
        public IZoom_Meeting_Repo zoomMeeting_Repo                  { get; private set; }
        public IWeb_Page_Repo page_Repo                               { get; private set; }
        public IBlog_Repo blog_Repo                               { get; private set; }
        public IContact_Page_Repo ContactPage_                     { get; private set; }
        public ITenants_Repo tenants_Repo                         { get; private set; }
        public IDesignation_Repo Designation_Repo                 { get; private set; }
        public IChat_Repo chat_Repo                               { get; private set; }
        public IApple_Pass_Account_Repo Apple_Pass_Account        { get; private set; }
        public IUser_Batch_Process_Repo Process_Repo              { get; private set; }
        public IWallet_Pass_Repo      Wallet_Pass               { get; private set; }
        public IAccount_Transaction_Repo Account_Transaction      { get; private set; }
        public IAccount_Balance_Repo Account_Balance              { get; private set; }
        public IPass_Transmission_Repo Pass_Transaction            { get; private set; }

        public IUser_Vault_Repo User_Vault { get; private set; }

        public IArenas_Billing_Repo Arenas_Billing { get; private set; }

        public async void Dispose()
        {
            GC.SuppressFinalize(this);
            await _crmContext.DisposeAsync();
        }
        public async Task<int> Save()
        {
            return await _crmContext.SaveChangesAsync();
        }
    }
}
