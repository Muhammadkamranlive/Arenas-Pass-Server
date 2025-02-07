using System;
using Server.UOW;
using Server.Domain;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Tenant_License_Keys_Service : Base_Service<TenantLicenes>, ITenant_License_Keys_Service
    {
        private readonly ITenant_License_Keys_Repo        iRepo;
        private readonly KeyManagementService        keyService;
        private readonly IGet_Tenant_Id_Service      tenantId_Service;
        private readonly ITenant_Key_History_Service tkhService;
        public Tenant_License_Keys_Service
        (
            IUnit_Of_Work_Repo unitOfWork,
            ITenant_License_Keys_Repo genericRepository,
            KeyManagementService key,
            IGet_Tenant_Id_Service get_Tenant_,
            ITenant_Key_History_Service tkh
        ) : base(unitOfWork, genericRepository)
        {
            iRepo            = genericRepository;
            keyService       = key;
            tenantId_Service = get_Tenant_;
            tkhService       = tkh;
        }


        /// <summary>
        /// License Key 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<TenantLicenes> CreateOrUpdateKeysForTenant()
        {

            int tenantId   =  tenantId_Service.GetTenantId();
            var (publicKey, privateKey) = keyService.GenerateKeyPair(tenantId);

            // Check if tenant already has keys
            var existingKeys   = await iRepo.FindOne(x=>x.TenantId == tenantId);
            if (existingKeys == null)
            {
                // Create new entry
                var newLicense = new TenantLicenes
                {
                    TenantId    = tenantId,
                    publicKey   = publicKey,
                    privateKey  = keyService.EncryptPrivateKey(privateKey),
                    Status      = "Active",
                    CreatedAt   = DateTime.Now,
                    Update_At   = DateTime.Now
                };

                await iRepo.Add(newLicense);
                await iRepo.Save();
                return newLicense;
            }
            else
            {
                // Update existing keys
                existingKeys.publicKey  = publicKey;
                existingKeys.privateKey = keyService.EncryptPrivateKey(privateKey);
                existingKeys.Update_At  = DateTime.Now;
                
                iRepo.Update(existingKeys);
                var apikeyHistory = new TenantKeyHistory()
                {
                    Change_By  = tenantId_Service.GetUserId(),
                    CreatedAt  = DateTime.Now,
                    privateKey = existingKeys.privateKey,
                    publicKey  = existingKeys.publicKey,
                    TenantId   = tenantId
                };
                await tkhService.InsertAsync(apikeyHistory);
                await iRepo.Save();
                return existingKeys;
            }
        }

        public async Task<TenantLicenes?> GetApiKeys()
        {
            int tenantId = tenantId_Service.GetTenantId();
           return await iRepo.FindOne(x => x.TenantId == tenantId);
        }
    }
}
