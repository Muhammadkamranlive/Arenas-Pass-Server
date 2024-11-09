using Server.Models;
using Passbook.Generator;
using Passbook.Generator.Fields;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace Server.Services
{
    public class Apple_Passes_Service : IApple_Passes_Service
    {

        #region Constructor
        private readonly Certificate_Settings_Model _certificateSettings;
        private readonly AuthManager               _authManager;
        public Apple_Passes_Service
        (
          IOptions<Certificate_Settings_Model> certificateSettings,
          AuthManager                          authManager
        )
        {
            _certificateSettings  = certificateSettings.Value;
            _authManager          = authManager;
        }

        #endregion


        
        /// <summary>
        /// GiftCard Generation General
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GiftCards(Gift_Card_Pass_Model GiftCard)
        {
			try
			{
                 ResponseModel<string> giftResponse = new()
                 {
                    Status_Code                      = "200",
                    Description                      = "OK",
                    Response                         = null
                 };

                //validations
                // Validate required fields
                if (GiftCard == null)
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "GiftCard model cannot be null";
                    return giftResponse;
                }
                else if (string.IsNullOrEmpty(GiftCard.Team_Identifier) || string.IsNullOrWhiteSpace(GiftCard.Team_Identifier))
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Team Identifier is required.";
                    return giftResponse;
                }
                

                else if (string.IsNullOrEmpty(GiftCard.Serial_Number) ||string.IsNullOrWhiteSpace(GiftCard.Serial_Number))
                {
                   
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Serial Number is required.";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(GiftCard.Organization_Name) || string.IsNullOrEmpty(GiftCard.Organization_Name))
                {
                   
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Organization Name is required.";
                    return giftResponse;
                }
                   

                else if (string.IsNullOrWhiteSpace(GiftCard.Description) || string.IsNullOrEmpty(GiftCard.Description))
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Description is required.";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(GiftCard.Balance) || string.IsNullOrEmpty(GiftCard.Balance)) 
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Balance is required.";
                    return giftResponse;
                }

                var appleCertificatePath         = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath      = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword  = _certificateSettings.PassbookCertificatePassword;

                PassGenerator generator      = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier       = "pass.com.faces2.ca",
                    TeamIdentifier           = GiftCard.Team_Identifier,
                    SerialNumber             = GiftCard.Serial_Number,
                    Description              = GiftCard.Description,
                    OrganizationName         = GiftCard.Organization_Name,
                    LogoText                 = GiftCard.Logo_Text,
                    BackgroundColor          = GiftCard.Background_Color,  
                    LabelColor               = GiftCard.Label_Color,               
                    ForegroundColor          = GiftCard.Foreground_Color,       
                    Style                    = PassStyle.StoreCard,
                    
                    //Addtional Properties not Understand able
                    //WebServiceUrl            = ,
                    //AppLaunchURL             = ,
                    //AuthenticationToken      = ,
                    //Images                   = ,
                    //HeaderFields             = ,
                    //AssociatedStoreIdentifiers=,
                    //Nfc                       =,
                    //RelevantBeacons           =,
                    //TransitType               =,
                    //Localizations             =,
                    //SuppressStripShine        =,
                    //UserInfo                  =
                };

                // Add primary field (Balance)
                request.AddPrimaryField(new StandardField("balance", "Balance", GiftCard.Balance)
                {
                    AttributedValue   = GiftCard.Balance,
                    DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                });

                // Add secondary field (Cardholder Name)
                if(!string.IsNullOrEmpty(GiftCard.Card_holder_Name) || !string.IsNullOrWhiteSpace(GiftCard.Card_holder_Name))
                {
                   request.AddSecondaryField(new StandardField("cardholder-name", GiftCard.Card_Holder_Title, GiftCard.Card_holder_Name)
                   {
                       AttributedValue = GiftCard.Card_holder_Name,
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                   });
                }

                // Add secondary field (Expiry Date)
                if (GiftCard.Expiry_Date!=null)
                {
                   request.AddBackField(new StandardField("expiry-date", "Expiry Date", GiftCard.Expiry_Date.ToString("MM/dd/yyyy"))
                   {
                       AttributedValue = GiftCard.Expiry_Date.ToString("MM/dd/yyyy"),
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                   });
                }
                

                   
                // Add auxiliary fields
                if(!string.IsNullOrWhiteSpace(GiftCard.Email) || !string.IsNullOrEmpty(GiftCard.Email))
                {
                   request.AddBackField(new StandardField("email", "Email", GiftCard.Email)
                   {
                       AttributedValue   = GiftCard.Email,
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                   });
                }
                
                if (!string.IsNullOrWhiteSpace(GiftCard.Email) || !string.IsNullOrEmpty(GiftCard.Email))
                {
                   request.AddBackField(new StandardField("phone", "Phone", GiftCard.Phone ?? "N/A")
                   {
                       AttributedValue   = GiftCard.Phone ?? "N/A",
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                   });
                }

                // Add auxiliary field (Terms and Conditions)
                if(!string.IsNullOrEmpty(GiftCard.Privacy_Policy) || !string.IsNullOrWhiteSpace(GiftCard.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", GiftCard.Privacy_Policy ?? "https://yourtermslink.com")
                    {
                        AttributedValue   = GiftCard.Privacy_Policy ?? "https://yourtermslink.com",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(GiftCard.Code_Type) && GiftCard.Code_Type=="BAR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + GiftCard.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + GiftCard.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(GiftCard.Code_Type) && GiftCard.Code_Type=="QR")
                {
                     request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + GiftCard.Card_holder_Name, "ISO-8859-1", GiftCard.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(GiftCard.Logo_Url) || !string.IsNullOrEmpty(GiftCard.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(GiftCard.Logo_Url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                logoImageBytes = await response.Content.ReadAsByteArrayAsync();
                                request.Images.Add(PassbookImage.Logo, logoImageBytes);
                            }
                        }
                    }
                }

                //Additional Properties as Back
                if(!string.IsNullOrEmpty(GiftCard.Address) || !string.IsNullOrWhiteSpace(GiftCard.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", GiftCard.Address)
                    {
                        AttributedValue   = GiftCard.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }
                 
                if(!string.IsNullOrEmpty(GiftCard.Webiste) || !string.IsNullOrWhiteSpace(GiftCard.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", GiftCard.Webiste)
                    {
                        AttributedValue   = GiftCard.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                
                if(!string.IsNullOrEmpty(GiftCard.Notes) || !string.IsNullOrWhiteSpace(GiftCard.Notes))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", GiftCard.Notes)
                    {
                        AttributedValue   = GiftCard.Notes,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
               

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags        = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate   = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate      = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass             = generator.Generate(request);
                giftResponse.Response            = generatedPass;

                return giftResponse;
            }
			catch (Exception ex)
			{

                ResponseModel<string> giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response    = "Error Occurred"
                };
                return giftResponse;
			}
        }
   
    
    }
}
