using Server.UOW;
using Server.Core;
using Server.Domain;
using Server.Models;
using Server.Repository;
using Server.BaseService;

namespace Server.Services
{
    public class Voucher_Service : Base_Service<Voucher>, IVoucher_Service
    {
        private readonly IApple_Passes_Service _applePasses_Service;
        private readonly IVoucher_Repo         _voucher_Repo;
        public Voucher_Service
        (
            IUnitOfWork unitOfWork,
            IApple_Passes_Service ap_Service,
            IVoucher_Repo iRepo
        ) : base(unitOfWork, iRepo)
        {
            _applePasses_Service = ap_Service;
            _voucher_Repo        = iRepo;
        }


        /// <summary>
        /// Generate Vouchers
        /// </summary>
        /// <param name="Voucher"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateVoucher(Apple_Passes_Voucher_Model VoucherModel)
        {
            try
            {
                ResponseModel<string> voucherResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                // Getting Pass
                voucherResponse  = _applePasses_Service.Vouchers(VoucherModel);
                if (voucherResponse.Status_Code != "200") { return voucherResponse; }

                Voucher voucher              = new();
                voucher.Type                 = "Voucher";
                voucher.Apple_Pass           = (byte[])voucherResponse.Response;
                voucher.Background_Color     = VoucherModel.Background_Color;
                voucher.Label_Color          = VoucherModel.Label_Color;
                voucher.Foreground_Color     = VoucherModel.Foreground_Color;
                voucher.Localized_Name       = "N/A";
                voucher.Terms_And_Conditions = "N/A";
                voucher.Logo                 = "N/A";
                voucher.Organization_Name    = VoucherModel.Organization_Name;
                voucher.Serial_Number        = VoucherModel.Serial_Number;
                voucher.Description          = VoucherModel.Description;
                voucher.Web_Service_URL      = "N/A";
                voucher.Authentication_Token = "";

                // Set properties specific to Voucher
                voucher.Voucher_Code    = "N/A";
                voucher.Issuer          = "N/A";
                voucher.Amount          = !string.IsNullOrEmpty(VoucherModel.Amount) ? decimal.Parse(VoucherModel.Amount) : 0;
                voucher.Currency_Code   = "N/A";
                voucher.Expiration_Date = VoucherModel.Expiry_Date;

                //Saving
                await _voucher_Repo.Transaction();
                await _voucher_Repo.Add(voucher);
                await _voucher_Repo.Commit();

                return voucherResponse;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response    = "Error Occurred"
                };
            }
        }



    }
}
