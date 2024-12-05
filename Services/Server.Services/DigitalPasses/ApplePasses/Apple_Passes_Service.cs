using Server.Models;
using Server.Domain;
using Passbook.Generator;
using System.Reflection.Emit;
using Passbook.Generator.Fields;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace Server.Services
{
    public class Apple_Passes_Service : IApple_Passes_Service
    {

        #region Constructor
        private readonly Certificate_Settings_Model _certificateSettings;
       
        public Apple_Passes_Service
        (
          IOptions<Certificate_Settings_Model> certificateSettings
        )
        {
            _certificateSettings  = certificateSettings.Value;
            
        }

        #endregion
        
        /// <summary>
        /// model Generation Passes Apple
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GiftCards(Apple_Passes_Gift_Card_Model model, string serialNo)
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
                if (model == null)
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "model model cannot be null";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(serialNo) || string.IsNullOrEmpty(serialNo))
                {

                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Serial Number is required";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(model.Logo_Text) || string.IsNullOrEmpty(model.Logo_Text))
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Logo is required.";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(model.Balance) || string.IsNullOrEmpty(model.Balance)) 
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
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = serialNo,
                    Description              = model.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = model.Logo_Text,
                    BackgroundColor          = model.Background_Color,  
                    LabelColor               = model.Label_Color,               
                    ForegroundColor          = model.Foreground_Color,       
                    Style                    = PassStyle.StoreCard,
                    
                };

                // Add primary field (Balance)
                request.AddPrimaryField(new StandardField("balance", "Balance", model.Balance)
                {
                    AttributedValue   = model.Balance,
                    DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                });

                // Add secondary field (Cardholder Name)
                if(!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                   request.AddSecondaryField(new StandardField("cardholder-name", model.Card_Holder_Title, model.Card_holder_Name)
                   {
                       AttributedValue = model.Card_holder_Name,
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                   });
                }

                
                if (model.Expiration_Date!=null)
                {
                   request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                   {
                       AttributedValue   = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                   });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                   request.AddBackField(new StandardField("email", "Email", model.Email)
                   {
                       AttributedValue   = model.Email,
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                   });
                }
                
                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                   request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                   {
                       AttributedValue   = model.Phone ?? "N/A",
                       DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                   });
                }

                // Add auxiliary field (Terms and Conditions)
                if(!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type=="BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type=="QR")
                {
                     request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if(!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }
                 
                if(!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                
                if(!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue = model.Issuer,
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
                giftResponse.Description         = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();

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


        /// <summary>
        /// Coupon Generation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> Coupons(Apple_Passes_Coupon_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier       = "pass.com.faces2.ca",
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = SerialNo,
                    Description              = model.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = model.Logo_Text,
                    BackgroundColor          = model.Background_Color,  
                    LabelColor               = model.Label_Color,               
                    ForegroundColor          = model.Foreground_Color,   
                    Style                    = PassStyle.Coupon
                };

                // Primary Field: Discount
                request.AddPrimaryField(new StandardField("discount", "Discount", model.Discount_Percentage.ToString())
                {
                    AttributedValue = model.Discount_Percentage.ToString()
                });

                //Secondary Field
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddSecondaryField(new StandardField("cardholder-name", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue   = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                //Back Properties
                if (!string.IsNullOrEmpty(model.Offer_Code))
                {
                    request.AddBackField(new StandardField("offer-Code", "Offer Code", model.Offer_Code)
                    {
                        AttributedValue   = model.Offer_Code,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                if (!string.IsNullOrEmpty(model.Is_Redeemed))
                {
                    request.AddBackField(new StandardField("redeem-status", "Redeem Status", model.Is_Redeemed)
                    {
                        AttributedValue   = model.Is_Redeemed,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response = "Error Occurred"
                };
            }
        }

        /// <summary>
        /// Voucher Generation
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> Vouchers(Apple_Passes_Voucher_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier       = "pass.com.faces2.ca",
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = SerialNo,
                    Description              = model.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = model.Logo_Text,
                    BackgroundColor          = model.Background_Color,  
                    LabelColor               = model.Label_Color,               
                    ForegroundColor          = model.Foreground_Color,   
                    Style                    = PassStyle.Generic
                };

                // Primary Field: Discount
                request.AddPrimaryField(new StandardField("discount", model.Offer, model.Amount.ToString())
                {
                    AttributedValue = model.Amount.ToString()
                });

                //Secondary Field
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddSecondaryField(new StandardField("cardholder-name", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue   = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                //Back Properties
                if (!string.IsNullOrEmpty(model.Currency_Code))
                {
                    request.AddBackField(new StandardField("Currency_Code", "Currency", model.Currency_Code)
                    {
                        AttributedValue   = model.Currency_Code,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                if (!string.IsNullOrEmpty(model.Is_Redeemed))
                {
                    request.AddBackField(new StandardField("redeem-status", "Redeem Status", model.Is_Redeemed)
                    {
                        AttributedValue   = model.Is_Redeemed,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
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


        
        /// <summary>
        /// Generate Loyalty Apple Pass
        /// </summary>
        /// <param name="loyaltyCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateLoyaltyCard(Apple_Passes_Loyalty_Card_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier       = "pass.com.faces2.ca",
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = SerialNo,
                    Description              = model.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = model.Logo_Text,
                    BackgroundColor          = model.Background_Color,  
                    LabelColor               = model.Label_Color,               
                    ForegroundColor          = model.Foreground_Color,   
                    Style                    = PassStyle.Generic
                };

                // Primary Field: Discount
                request.AddPrimaryField(new StandardField("Program_Name", model.Program_Name, model.Points_Balance.ToString())
                {
                    AttributedValue = model.Points_Balance.ToString()
                });

                //Secondary Field
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddSecondaryField(new StandardField("cardholder-name", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue   = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                //Back Properties
                if (!string.IsNullOrEmpty(model.Reward_Details))
                {
                    request.AddBackField(new StandardField("Currency_Code", "Reward", model.Reward_Details)
                    {
                        AttributedValue   = model.Reward_Details,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                
                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message, Response = null };
            }
        }

        /// <summary>
        /// Membership Cards
        /// </summary>
        /// <param name="membershipCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateMembershipCard(Apple_Passes_Membership_Card_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier       = "pass.com.faces2.ca",
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = SerialNo,
                    Description              = model.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = model.Logo_Text,
                    BackgroundColor          = model.Background_Color,  
                    LabelColor               = model.Label_Color,               
                    ForegroundColor          = model.Foreground_Color,   
                    Style                    = PassStyle.Generic
                };

                // Primary Field: Discount
                request.AddPrimaryField(new StandardField("membership-number", "Membership Number", model.Membership_Number));
                request.AddPrimaryField(new StandardField("member-name", "Member Name", model.Member_Name));


                //Secondary Field
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddPrimaryField(new StandardField("membership-name", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue   = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                //Back Properties
                if (!string.IsNullOrEmpty(model.Additional_Benefits))
                {
                    request.AddBackField(new StandardField("Additional_Benefits", "Additional Benefits", model.Additional_Benefits)
                    {
                        AttributedValue   = model.Additional_Benefits,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                
                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message, Response = null };
            }
        }

        /// <summary>
        /// Tickets
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateTicket(Apple_Passes_Ticket_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier          = "pass.com.faces2.ca",
                    TeamIdentifier              = "ArenasPass@DevTeam",
                    SerialNumber                = SerialNo,
                    Description                 = model.Description,
                    OrganizationName            = "ArenasPass",
                    LogoText                    = model.Logo_Text,
                    BackgroundColor             = model.Background_Color,  
                    LabelColor                  = model.Label_Color,               
                    ForegroundColor             = model.Foreground_Color,   
                    Style                       = PassStyle.EventTicket
                };

                // Primary Field: Discount
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddPrimaryField(new StandardField("Event", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }


                //Secondary Field
                if (model.Effective_Date != null)
                {
                    request.AddSecondaryField(new StandardField("date", "Date", model.Effective_Date.Value.ToString("MMMM dd, yyyy hh:mm tt")));
                }
                if (!string.IsNullOrEmpty(model.VenueName))
                {
                    request.AddSecondaryField(new StandardField("venue", "Venue", model.VenueName));
                }
                if (!string.IsNullOrEmpty(model.SeatInfo))
                {
                    request.AddAuxiliaryField(new StandardField("seat", "Seat", model.SeatInfo ?? "General"));
                }

                if (!string.IsNullOrEmpty(model.EntryGate))
                {
                    request.AddAuxiliaryField(new StandardField("gate", "Gate", model.EntryGate ?? "N/A"));
                }
                //Back Properties
                if (!string.IsNullOrEmpty(model.TicketNumber))
                {
                    request.AddBackField(new StandardField("TicketNumber", "Ticket Number", model.TicketNumber)
                    {
                        AttributedValue   = model.TicketNumber,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone,
                    });
                }
                
                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message, Response = null };
            }
        }




        /// <summary>
        /// Punch Cards
        /// </summary>
        /// <param name="punchCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> PunchCards(Apple_Passes_Punch_Card_Model model,string SerialNo)
        {
            try
            {
                ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier          = "pass.com.faces2.ca",
                    TeamIdentifier              = "ArenasPass@DevTeam",
                    SerialNumber                = SerialNo,
                    Description                 = model.Description,
                    OrganizationName            = "ArenasPass",
                    LogoText                    = model.Logo_Text,
                    BackgroundColor             = model.Background_Color,  
                    LabelColor                  = model.Label_Color,               
                    ForegroundColor             = model.Foreground_Color,   
                    Style                       = PassStyle.Generic
                };

                // Primary Field: Discount
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddPrimaryField(new StandardField("Card_Holder_Title", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Secondary Field
                request.AddSecondaryField(new StandardField("progress", "Progress", $"{model.Current_Punches}/{model.Total_Punches}"));

                if (!string.IsNullOrWhiteSpace(model.Reward_Details) || !string.IsNullOrEmpty(model.Reward_Details))
                {
                    request.AddBackField(new StandardField("Reward_Details", "Reward Details", model.Reward_Details ?? "N/A")
                    {
                        AttributedValue = model.Reward_Details ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
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

        /// <summary>
        /// Boarding Pass
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> BoardingPass(Boarding_Pass_Model model, string SerialNo)
        {
            try
            {
                 ResponseModel<string> passResponse = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (model == null || string.IsNullOrEmpty(SerialNo) || string.IsNullOrEmpty(SerialNo))
                {
                    passResponse.Status_Code = "400";
                    passResponse.Description = "Coupon model and required fields cannot be null.";
                    return passResponse;
                }

                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
                PassGenerator generator         = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier          = "pass.com.faces2.ca",
                    TeamIdentifier              = "ArenasPass@DevTeam",
                    SerialNumber                = SerialNo,
                    Description                 = model.Description,
                    OrganizationName            = "ArenasPass",
                    LogoText                    = model.Logo_Text,
                    BackgroundColor             = model.Background_Color,  
                    LabelColor                  = model.Label_Color,               
                    ForegroundColor             = model.Foreground_Color,   
                    Style                       = PassStyle.BoardingPass
                };

                // Primary Field: Discount
                if (!string.IsNullOrEmpty(model.Card_holder_Name) || !string.IsNullOrWhiteSpace(model.Card_holder_Name))
                {
                    request.AddPrimaryField(new StandardField("Card_Holder_Title", model.Card_Holder_Title, model.Card_holder_Name)
                    {
                        AttributedValue = model.Card_holder_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                // Add Primary Fields
                request.AddPrimaryField(new StandardField("flightNumber", "Flight", model.FlightNumber)
                {
                    AttributedValue = model.FlightNumber
                });
                request.AddPrimaryField(new StandardField("route", "Route", $"{model.DepartureAirportCode} → {model.ArrivalAirportCode}")
                {
                    AttributedValue = $"{model.DepartureAirportCode} → {model.ArrivalAirportCode}"
                });

                // Add Secondary Fields
                request.AddSecondaryField(new StandardField("departureTime", "Departure Time", model.DepartureTime.ToString("hh:mm tt"))
                {
                    AttributedValue = model.DepartureTime.ToString("hh:mm tt")
                });
                request.AddSecondaryField(new StandardField("boardingTime", "Boarding Time", model.BoardingTime.ToString("hh:mm tt"))
                {
                    AttributedValue = model.BoardingTime.ToString("hh:mm tt")
                });
                request.AddSecondaryField(new StandardField("gate", "Gate", model.GateNumber ?? "TBD")
                {
                    AttributedValue = model.GateNumber ?? "TBD"
                });
                request.AddSecondaryField(new StandardField("terminal", "Terminal", model.Terminal ?? "TBD")
                {
                    AttributedValue = model.Terminal ?? "TBD"
                });

                // Add Auxiliary Fields
                request.AddAuxiliaryField(new StandardField("seat", "Seat", model.SeatNumber ?? "Unassigned")
                {
                    AttributedValue = model.SeatNumber ?? "Unassigned"
                });
                request.AddAuxiliaryField(new StandardField("class", "Class", model.ClassOfService ?? "N/A")
                {
                    AttributedValue = model.ClassOfService ?? "N/A"
                });
                request.AddAuxiliaryField(new StandardField("passenger", "Passenger", model.PassengerName)
                {
                    AttributedValue = model.PassengerName
                });
                if (!string.IsNullOrEmpty(model.FrequentFlyerNumber))
                {
                    request.AddAuxiliaryField(new StandardField("frequentFlyer", "Frequent Flyer", model.FrequentFlyerNumber)
                    {
                        AttributedValue = model.FrequentFlyerNumber
                    });
                }

                // Add Back Fields
                if (model.DepartureTime != null)
                {
                    request.AddBackField(new StandardField("departureDate", "Departure Date", model.DepartureTime.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.DepartureTime.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.AirlineName))
                {
                    request.AddBackField(new StandardField("airline", "Airline", model.AirlineName)
                    {
                        AttributedValue = model.AirlineName
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.BaggageClaimInfo))
                {
                    request.AddBackField(new StandardField("baggage", "Baggage Info", model.BaggageClaimInfo)
                    {
                        AttributedValue = model.BaggageClaimInfo
                    });
                }
                //Secondary Field

                if (model.Expiration_Date != null)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", model.Expiration_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Expiration_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Relevant_Date != null)
                {
                    request.AddBackField(new StandardField("relevant-date", "Relevant Date", model.Relevant_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = model.Relevant_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (model.Effective_Date != null)
                {
                    request.AddBackField(new StandardField("effective-date", "Effective Date", model.Effective_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue = model.Effective_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Email) || !string.IsNullOrEmpty(model.Email))
                {
                    request.AddBackField(new StandardField("email", "Email", model.Email)
                    {
                        AttributedValue   = model.Email,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Phone) || !string.IsNullOrEmpty(model.Phone))
                {
                    request.AddBackField(new StandardField("phone", "Phone", model.Phone ?? "N/A")
                    {
                        AttributedValue   = model.Phone ?? "N/A",
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypePhoneNumber
                    });
                }

                // Add auxiliary field (Terms and Conditions)
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", model.Privacy_Policy)
                    {
                        AttributedValue   = model.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }
                if (!string.IsNullOrEmpty(model.Privacy_Policy) || !string.IsNullOrWhiteSpace(model.Privacy_Policy))
                {
                    request.AddBackField(new StandardField("privacy", "Privacy Policy", model.Terms_And_Conditions)
                    {
                        AttributedValue   = model.Terms_And_Conditions,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + model.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + model.Team_Identifier);
                }

                if (!string.IsNullOrEmpty(model.Code_Type) && model.Code_Type == "QR")
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatQR, "https://touchconnectapp.web.app/" + model.Card_holder_Name, "ISO-8859-1", model.Card_holder_Name);
                }

                // Fetch and add images (e.g., logo)
                if (!string.IsNullOrWhiteSpace(model.Logo_Url) || !string.IsNullOrEmpty(model.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(model.Logo_Url))
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
                if (!string.IsNullOrEmpty(model.Address) || !string.IsNullOrWhiteSpace(model.Address))
                {
                    request.AddBackField(new StandardField("address", "Address", model.Address)
                    {
                        AttributedValue   = model.Address,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeAddress
                    });
                }

                if (!string.IsNullOrEmpty(model.Webiste) || !string.IsNullOrWhiteSpace(model.Webiste))
                {
                    // Add Website back field
                    request.AddBackField(new StandardField("website", "Website", model.Webiste)
                    {
                        AttributedValue   = model.Webiste,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                if (!string.IsNullOrEmpty(model.Message) || !string.IsNullOrWhiteSpace(model.Message))
                {
                    // Add Notes back field
                    request.AddBackField(new StandardField("notes", "Notes", model.Message)
                    {
                        AttributedValue   = model.Message,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    request.AddBackField(new StandardField("description", "Description", model.Description)
                    {
                        AttributedValue   = model.Description,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                //Sender /Reciever 
                if (!string.IsNullOrWhiteSpace(model.Sender_Name))
                {
                    request.AddBackField(new StandardField("sender", "Frome", model.Sender_Name)
                    {
                        AttributedValue   = model.Sender_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }
                if (!string.IsNullOrWhiteSpace(model.Recipient_Name))
                {
                    request.AddBackField(new StandardField("description", "Receiver", model.Recipient_Name)
                    {
                        AttributedValue = model.Recipient_Name,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                if (!string.IsNullOrWhiteSpace(model.Issuer))
                {
                    request.AddBackField(new StandardField("Issuer", "Issuer", model.Issuer)
                    {
                        AttributedValue   = model.Issuer,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorNone
                    });
                }

                // Load certificates with appropriate flags for security
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass     = generator.Generate(request);
                passResponse.Response    = generatedPass;
                passResponse.Description = model.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();
                return passResponse;
            }
            catch (Exception ex)
            {

                ResponseModel<string> giftResponse = new ResponseModel<string>()
                {
                    Status_Code = "500",
                    Description = ex.Message,
                    Response = "Error Occurred"
                };
                return giftResponse;
            }
        }
    }
}
