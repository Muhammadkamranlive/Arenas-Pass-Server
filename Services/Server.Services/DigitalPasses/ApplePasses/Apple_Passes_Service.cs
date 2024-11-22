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
       
        public Apple_Passes_Service
        (
          IOptions<Certificate_Settings_Model> certificateSettings
        )
        {
            _certificateSettings  = certificateSettings.Value;
            
        }

        #endregion
        
        /// <summary>
        /// GiftCard Generation Passes Apple
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GiftCards(Apple_Passes_Gift_Card_Model GiftCard, string serialNo)
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

                else if (string.IsNullOrWhiteSpace(serialNo) || string.IsNullOrEmpty(serialNo))
                {

                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Serial Number is required";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(GiftCard.Logo_Text) || string.IsNullOrEmpty(GiftCard.Logo_Text))
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Logo is required.";
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
                    TeamIdentifier           = "ArenasPass@DevTeam",
                    SerialNumber             = serialNo,
                    Description              = GiftCard.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = GiftCard.Logo_Text,
                    BackgroundColor          = GiftCard.Background_Color,  
                    LabelColor               = GiftCard.Label_Color,               
                    ForegroundColor          = GiftCard.Foreground_Color,       
                    Style                    = PassStyle.StoreCard,
                    
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
                   request.AddBackField(new StandardField("expiry-date", "Expiry Date", GiftCard.Expiry_Date.Value.ToString("MM/dd/yyyy"))
                   {
                       AttributedValue   = GiftCard.Expiry_Date.Value.ToString("MM/dd/yyyy"),
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
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", GiftCard.Privacy_Policy)
                    {
                        AttributedValue   = GiftCard.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(GiftCard.Code_Type) && GiftCard.Code_Type=="BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + GiftCard.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + GiftCard.Team_Identifier);
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
                giftResponse.Description         = GiftCard.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();

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
        /// <param name="coupon"></param>
        /// <returns></returns>
        public ResponseModel<string> Coupons(Apple_Passes_Coupon_Model coupon)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response = null
                };

                if (coupon == null || string.IsNullOrEmpty(coupon.Serial_Number) || string.IsNullOrEmpty(coupon.Description))
                {
                    response.Status_Code = "400";
                    response.Description = "Coupon model and required fields cannot be null.";
                    return response;
                }

                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.example.coupon",
                    TeamIdentifier     = coupon.Team_Identifier,
                    SerialNumber       = coupon.Serial_Number,
                    Description        = coupon.Description,
                    OrganizationName   = coupon.Organization_Name,
                    BackgroundColor    = coupon.Background_Color,
                    LabelColor         = coupon.Label_Color,
                    ForegroundColor    = coupon.Foreground_Color,
                    Style              = PassStyle.Coupon
                };

                // Primary Field: Discount
                request.AddPrimaryField(new StandardField("discount", "Discount", coupon.Discount)
                {
                    AttributedValue = coupon.Discount
                });

                // Back Fields: Terms, Expiry Date
                request.AddBackField(new StandardField("expiry", "Expires", coupon.Expiry_Date.ToString("MM/dd/yyyy")));
                request.AddBackField(new StandardField("terms", "Terms", coupon.Terms));

                // Barcode
                request.AddBarcode(BarcodeType.PKBarcodeFormatQR, coupon.Serial_Number, "ISO-8859-1", coupon.Serial_Number);
                response.Response = GeneratePass(request, response);
                return response;
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
        public ResponseModel<string> Vouchers(Apple_Passes_Voucher_Model voucher)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (voucher == null || string.IsNullOrEmpty(voucher.Serial_Number) || string.IsNullOrEmpty(voucher.Organization_Name))
                {
                    response.Status_Code = "400";
                    response.Description = "Voucher model and required fields cannot be null.";
                    return response;
                }

                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.example.voucher",
                    TeamIdentifier     = voucher.Team_Identifier,
                    SerialNumber       = voucher.Serial_Number,
                    Description        = voucher.Description,
                    OrganizationName   = voucher.Organization_Name,
                    BackgroundColor    = voucher.Background_Color,
                    LabelColor         = voucher.Label_Color,
                    ForegroundColor    = voucher.Foreground_Color,
                    Style              = PassStyle.Generic
                };

                // Primary Field: Voucher Amount
                request.AddPrimaryField(new StandardField("amount", "Amount", voucher.Amount)
                {
                    AttributedValue = voucher.Amount
                });

                // Back Fields: Expiry Date
                request.AddBackField(new StandardField("expiry", "Expires", voucher.Expiry_Date.ToString("MM/dd/yyyy")));

                // Barcode
                request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, voucher.Serial_Number, "ISO-8859-1", voucher.Serial_Number);

                response.Response = GeneratePass(request, response);
                return response;
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
        /// Generate Pass Hash
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private ResponseModel<string> GeneratePass(PassGeneratorRequest request, ResponseModel<string> response)
        {
            var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
            var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
            var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;
            request.AppleWWDRCACertificate  = new X509Certificate2(appleCertificatePath);
            request.PassbookCertificate     = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            PassGenerator generator         = new PassGenerator();
            byte[] generatedPass            = generator.Generate(request);
            response.Response               = generatedPass;
            return response;
        }


        /// <summary>
        /// Generate Loyalty Apple Pass
        /// </summary>
        /// <param name="loyaltyCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> GenerateLoyaltyCard(Apple_Passes_Loyalty_Card_Model loyaltyCard)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                // Validate required fields
                if (loyaltyCard == null || string.IsNullOrEmpty(loyaltyCard.Team_Identifier) ||
                    string.IsNullOrEmpty(loyaltyCard.Serial_Number) || string.IsNullOrEmpty(loyaltyCard.Organization_Name))
                {
                    return new ResponseModel<string>
                    {
                        Status_Code = "400",
                        Description = "Required fields are missing.",
                        Response = null
                    };
                }

                // Initialize request
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.yourcompany.loyalty",
                    TeamIdentifier     = loyaltyCard.Team_Identifier,
                    SerialNumber       = loyaltyCard.Serial_Number,
                    OrganizationName   = loyaltyCard.Organization_Name,
                    LogoText           = loyaltyCard.Logo_Text,
                    BackgroundColor    = loyaltyCard.Background_Color,
                    LabelColor         = loyaltyCard.Label_Color,
                    ForegroundColor    = loyaltyCard.Foreground_Color,
                    Style              = PassStyle.Generic
                };

                // Add primary field (Points)
                request.AddPrimaryField(new StandardField("points", "Points", loyaltyCard.Points));

                // Add back fields for additional details
                request.AddBackField(new StandardField("membership-level", "Membership Level", loyaltyCard.Membership_Level ?? "N/A"));
                request.AddBackField(new StandardField("expiry-date", "Expiry Date", loyaltyCard.Expiry_Date?.ToString("MM/dd/yyyy") ?? "No Expiry"));

                // Add barcode
                request.AddBarcode(BarcodeType.PKBarcodeFormatQR, loyaltyCard.Barcode, "ISO-8859-1", loyaltyCard.Barcode);

                // Add images (logo)
                if (!string.IsNullOrWhiteSpace(loyaltyCard.Logo_Url))
                {
                    byte[] logoImageBytes = await FetchImageBytes(loyaltyCard.Logo_Url);
                    request.Images.Add(PassbookImage.Logo, logoImageBytes);
                }

                // Generate pass
                response = GeneratePass(request, response);
                return response;
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
        public ResponseModel<string> GenerateMembershipCard(Apple_Passes_Membership_Card_Model membershipCard,string serialNo)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.yourcompany.membership",
                    TeamIdentifier     = "pass.com.faces2.ca",
                    SerialNumber       = serialNo,
                    OrganizationName   = membershipCard.Organization_Name,
                    LogoText           = membershipCard.Logo_Text,
                    BackgroundColor    = membershipCard.Background_Color,
                    ForegroundColor    = membershipCard.Foreground_Color,
                    Style              = PassStyle.Generic
                };

                // Add primary field (Membership Type)
                request.AddPrimaryField(new StandardField("membership-type", "Membership Type", membershipCard.Membership_Type));

                // Add back fields (Details)
                request.AddBackField(new StandardField("expiry-date", "Expiry Date", membershipCard.Expiry_Date?.ToString("MM/dd/yyyy") ?? "No Expiry"));
                request.AddBackField(new StandardField("holder-name", "Holder Name", membershipCard.Holder_Name));

                // Add barcode
                request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, membershipCard.Barcode, "ISO-8859-1", membershipCard.Barcode);

                response = GeneratePass(request, response);
                return response;
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
        public ResponseModel<string> GenerateTicket(Apple_Passes_Ticket_Model ticket)
        {
            try
            {
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response    = null
                };

                if (ticket == null || string.IsNullOrEmpty(ticket.Team_Identifier) || string.IsNullOrEmpty(ticket.Serial_Number))
                {
                    return new ResponseModel<string>
                    {
                        Status_Code = "400",
                        Description = "Required fields are missing.",
                        Response    = null
                    };
                }

                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.yourcompany.ticket",
                    TeamIdentifier     = ticket.Team_Identifier,
                    SerialNumber       = ticket.Serial_Number,
                    OrganizationName   = ticket.Organization_Name,
                    LogoText           = ticket.Event_Name,
                    BackgroundColor    = ticket.Background_Color,
                    ForegroundColor    = ticket.Foreground_Color,
                    Style              = PassStyle.EventTicket
                };

                // Add primary field (Event Name)
                request.AddPrimaryField(new StandardField("event", "Event", ticket.Event_Name));

                // Add secondary field (Date & Time)
                request.AddSecondaryField(new StandardField("date-time", "Date & Time", ticket.Event_Date.ToString("MM/dd/yyyy hh:mm tt")));

                // Add auxiliary field (Seat)
                request.AddAuxiliaryField(new StandardField("seat", "Seat", ticket.Seat_Number));

                // Add barcode
                request.AddBarcode(BarcodeType.PKBarcodeFormatQR, ticket.Barcode, "ISO-8859-1", ticket.Barcode);

                response = GeneratePass(request,response);
                return response;
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status_Code = "500", Description = ex.Message, Response = null };
            }
        }

        /// <summary>
        /// Fetch Image
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<byte[]> FetchImageBytes(string url)
        {
            using HttpClient client            = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }


        /// <summary>
        /// Punch Cards
        /// </summary>
        /// <param name="punchCard"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> PunchCards(Apple_Passes_Punch_Card_Model punchCard)
        {
            try
            {
                ResponseModel<string> punchResponse = new()
                {
                    Status_Code = "200",
                    Description = "OK",
                    Response = null
                };

                // Validations
                if (punchCard == null)
                {
                    punchResponse.Status_Code = "404";
                    punchResponse.Description = "Punch card model cannot be null";
                    return punchResponse;
                }
                else if (string.IsNullOrEmpty(punchCard.Team_Identifier) || string.IsNullOrWhiteSpace(punchCard.Team_Identifier))
                {
                    punchResponse.Status_Code = "404";
                    punchResponse.Description = "Team Identifier is required.";
                    return punchResponse;
                }
                else if (string.IsNullOrEmpty(punchCard.Serial_Number) || string.IsNullOrWhiteSpace(punchCard.Serial_Number))
                {
                    punchResponse.Status_Code = "404";
                    punchResponse.Description = "Serial Number is required.";
                    return punchResponse;
                }
                else if (string.IsNullOrEmpty(punchCard.Organization_Name) || string.IsNullOrWhiteSpace(punchCard.Organization_Name))
                {
                    punchResponse.Status_Code = "404";
                    punchResponse.Description = "Organization Name is required.";
                    return punchResponse;
                }
                else if (punchCard.Total_Punches <= 0)
                {
                    punchResponse.Status_Code = "404";
                    punchResponse.Description = "Total punches must be greater than zero.";
                    return punchResponse;
                }

                // Load certificates
                var appleCertificatePath        = _certificateSettings.AppleWWDRCACertificatePath;
                var passbookCertificatePath     = _certificateSettings.PassbookCertificatePath;
                var passbookCertificatePassword = _certificateSettings.PassbookCertificatePassword;

                // Create Pass
                PassGenerator generator      = new PassGenerator();
                PassGeneratorRequest request = new PassGeneratorRequest
                {
                    PassTypeIdentifier = "pass.com.faces2.ca",
                    TeamIdentifier     = punchCard.Team_Identifier,
                    SerialNumber       = punchCard.Serial_Number,
                    Description        = $"Punch Card: {punchCard.Punch_Title}",
                    OrganizationName   = punchCard.Organization_Name,
                    LogoText           = punchCard.Logo_Text,
                    BackgroundColor    = punchCard.Background_Color,
                    LabelColor         = punchCard.Label_Color,
                    ForegroundColor    = punchCard.Foreground_Color,
                    Style              = PassStyle.Generic
                };

                // Primary Field: Punch Status
                request.AddPrimaryField(new StandardField("punch-status", "Punches", $"{punchCard.Current_Punches}/{punchCard.Total_Punches}")
                {
                    AttributedValue = $"{punchCard.Current_Punches}/{punchCard.Total_Punches}"
                });

                // Back Field: Reward Details
                if (!string.IsNullOrWhiteSpace(punchCard.Reward_Details))
                {
                    request.AddBackField(new StandardField("reward-details", "Reward", punchCard.Reward_Details));
                }

                // Back Field: Expiry Date
                if (punchCard.Expiry_Date.HasValue)
                {
                    request.AddBackField(new StandardField("expiry-date", "Expiry Date", punchCard.Expiry_Date.Value.ToString("MM/dd/yyyy"))
                    {
                        AttributedValue   = punchCard.Expiry_Date.Value.ToString("MM/dd/yyyy"),
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeCalendarEvent
                    });
                }

                // Barcode
                if (!string.IsNullOrEmpty(punchCard.Barcode))
                {
                    request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, punchCard.Barcode, "ISO-8859-1", punchCard.Barcode);
                }

                // Logo Image
                if (!string.IsNullOrWhiteSpace(punchCard.Logo_Url))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] logoImageBytes;
                        using (HttpResponseMessage response = await client.GetAsync(punchCard.Logo_Url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                logoImageBytes = await response.Content.ReadAsByteArrayAsync();
                                request.Images.Add(PassbookImage.Logo, logoImageBytes);
                            }
                        }
                    }
                }

                // Load certificates
                X509KeyStorageFlags flags      = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
                request.AppleWWDRCACertificate = new X509Certificate2(appleCertificatePath);
                request.PassbookCertificate    = new X509Certificate2(passbookCertificatePath, passbookCertificatePassword, flags);

                // Generate the pass
                byte[] generatedPass   = generator.Generate(request);
                punchResponse.Response = generatedPass;
                return punchResponse;
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
        /// Boarding Pass
        /// </summary>
        /// <param name="GiftCard"></param>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> BoardingPass(Apple_Passes_Gift_Card_Model GiftCard, string serialNo)
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
                    giftResponse.Description = "Boarding Pass model cannot be null";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(serialNo) || string.IsNullOrEmpty(serialNo))
                {

                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Serial Number is required";
                    return giftResponse;
                }

                else if (string.IsNullOrWhiteSpace(GiftCard.Logo_Text) || string.IsNullOrEmpty(GiftCard.Logo_Text))
                {
                    giftResponse.Status_Code = "404";
                    giftResponse.Description = "Logo is required.";
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
                    Description              = GiftCard.Description,
                    OrganizationName         = "ArenasPass",
                    LogoText                 = GiftCard.Logo_Text,
                    BackgroundColor          = GiftCard.Background_Color,  
                    LabelColor               = GiftCard.Label_Color,               
                    ForegroundColor          = GiftCard.Foreground_Color,       
                    Style                    = PassStyle.StoreCard,
                    
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
                   request.AddBackField(new StandardField("expiry-date", "Expiry Date", GiftCard.Expiry_Date.Value.ToString("MM/dd/yyyy"))
                   {
                       AttributedValue   = GiftCard.Expiry_Date.Value.ToString("MM/dd/yyyy"),
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
                    request.AddBackField(new StandardField("terms", "Terms and Conditions", GiftCard.Privacy_Policy)
                    {
                        AttributedValue   = GiftCard.Privacy_Policy,
                        DataDetectorTypes = DataDetectorTypes.PKDataDetectorTypeLink
                    });
                }

                // Add barcode with gift card code
                if (!string.IsNullOrEmpty(GiftCard.Code_Type) && GiftCard.Code_Type=="BAR")
                {
                    //request.AddBarcode(BarcodeType.PKBarcodeFormatCode128, "GIFT-50-USD-" + GiftCard.Team_Identifier, "ISO-8859-1", "GIFT-50-USD-" + GiftCard.Team_Identifier);
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
                giftResponse.Description         = GiftCard.Code_Type == "QR" ? BarcodeType.PKBarcodeFormatQR.ToString() : BarcodeType.PKBarcodeFormatCode128.ToString();

                return giftResponse;
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
