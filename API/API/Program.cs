
using API;
using Stripe;
using Server.UOW;
using Server.Core;
using API.RealTime;
using Autofac.Core;
using Server.Domain;
using Server.Mapper;
using Server.Models;
using Stripe.Issuing;
using Server.Services;
using Server.Repository;
using Server.Middlewares;
using API.TenantMiddleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Services.DigitalPasses;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Server.Services.DigitalPasses.Transaction;
using Server.Services.DigitalPasses.WalletPasses;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ERPDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"), x => x.MigrationsAssembly(typeof(Program).Assembly.FullName)));

builder.Services.AddIdentity<ApplicationUser, CustomRole>()
    .AddEntityFrameworkStores<ERPDb>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TenantResolve>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IPasswordReset_Repo, PasswordReset_Repo>();
builder.Services.AddScoped<IPasswordReset_Service, PasswordReset_Service>();
builder.Services.AddScoped<IEmail_Service, Email_Service>();
builder.Services.AddScoped<IContactDetails_Repo, ContactDetails_Repo>();
builder.Services.AddScoped<IEmergencyContacts_Repo, EmergencyContacts_Repo>();
builder.Services.AddScoped<ICaseCommetns_Repo, CaseCommetns_Repo>();
builder.Services.AddScoped<ICase_Repo, Case_Repo>();
builder.Services.AddScoped<IAssetManager_Repo, AssetManager_Repo>();
builder.Services.AddScoped<IAttachments_Repo, Attachments_Repo>();
builder.Services.AddScoped<IHRNotes_Repo, HRNotes_Repo>();
builder.Services.AddScoped<INotifications_Repo, Notifications_Repo>();
builder.Services.AddScoped<ICandidate_Repo, Candidate_Repo>();
builder.Services.AddScoped<IEducation_Repo, Education_Repo>();
builder.Services.AddScoped<IJobExperience_Repo, JobExperience_Repo>();
builder.Services.AddScoped<IProfessionalLicense_Repo, ProfessionalLicense_Repo>();
builder.Services.AddScoped<IPersonal_Repo, Personal_Repo>();
builder.Services.AddScoped<IGeneralTask_Repo, GeneralTask_Repo>();
builder.Services.AddScoped<ICaseComments_Service, CaseComments_Service>();
builder.Services.AddScoped<ICaseManagment_Service, CaseManagment_Service>();
builder.Services.AddScoped<IContactDetails_Service, ContactDetails_Service>();
builder.Services.AddScoped<IEmergencyContact_Service, EmergencyContact_Service>();
builder.Services.AddScoped<IAssetManager_Service, AssetManager_Service>();
builder.Services.AddScoped<IAttachments_Service, Attachments_Service>();
builder.Services.AddScoped<IGeneralTask_Service, GeneralTask_Service>();
builder.Services.AddScoped<IHRNotes_Service, HRNotes_Service>();
builder.Services.AddScoped<INotifications_Service, Notifications_Service>();
builder.Services.AddScoped<ICandidate_Service, Candidate_Service>();
builder.Services.AddScoped<IEducations_Service, Educations_Service>();
builder.Services.AddScoped<IJobExperience_Repo, JobExperience_Repo>();
builder.Services.AddScoped<IJobExperience_Service, JobExperience_Service>();
builder.Services.AddScoped<IProfile_Service, Profile_Service>();
builder.Services.AddScoped<IProfessionalLicense_Service, ProfessionalLicense_Service>();
builder.Services.AddScoped<IDependent_Repo, Dependent_Repo>();
builder.Services.AddScoped<IDependent_Service, Dependent_Service>();
builder.Services.AddScoped<IZoomService, ZoomService>();
builder.Services.AddScoped<IZoomMeting_Repo, ZoomMeting_Repo>();
builder.Services.AddScoped<IZoomMeeting_Service, ZoomMeeting_Service>();
builder.Services.AddScoped<IPage_Repo, Page_Repo>();
builder.Services.AddScoped<IWebPage_Service, WebPage_Service>();
builder.Services.AddScoped<IContactPage_Repo, ContactPage_Repo>();
builder.Services.AddScoped<IContactPage_Service, ContactPage_Service>();
builder.Services.AddScoped<IBlog_Repo, Blog_Repo>();
builder.Services.AddScoped<IBlog_Service, Blog_Service>();
builder.Services.AddScoped<ITraining_Repo, Training_Repo>();
builder.Services.AddScoped<ITraining_Service, Training_Service>();
builder.Services.AddScoped<ITenants_Repo, Tenants_Repo>();
builder.Services.AddScoped<ITenants_Service, Tenants_Service>();
builder.Services.AddScoped<ILog_Repo, Log_Repo>();
builder.Services.AddScoped<ILog_Service , Logs_Service>();
builder.Services.AddScoped<IDesignation_Repo, Designation_Repo>();
builder.Services.AddScoped<IDesignation_Service, Designation_Service>();
builder.Services.AddScoped<IChat_Repo, Chat_Repo>();
builder.Services.AddScoped<IChat_Service, Chat_Service>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IApple_Passes_Service, Apple_Passes_Service>();
builder.Services.AddScoped<IGift_Cards_Repo, Gift_Cards_Repo>();
builder.Services.AddScoped<IGift_Card_Service, Gift_Card_Service>();
builder.Services.AddScoped<IApple_Pass_Account_Repo, Apple_Pass_Account_Repo>();
builder.Services.AddScoped<IApple_Pass_Account_Service, Apple_Pass_Account_Service>();
builder.Services.AddScoped<ITransaction_No_Repo, Transaction_No_Repo>();
builder.Services.AddScoped<IGet_Tenant_Id_Service, Get_Tenant_Id_Service>();
builder.Services.AddScoped<ITransaction_No_Service, Transaction_No_Service>();
builder.Services.AddScoped<ITenant_Api_Hits_Repo, Tenant_Api_Hits_Repo>();
builder.Services.AddScoped<ITenant_Api_Hits_Service, Tenant_Api_Hists_Service>();
builder.Services.AddScoped<ITenant_Key_History_Repo, Tenant_Key_History_Repo>();
builder.Services.AddScoped<ITenant_Key_History_Service, Tenant_Key_History_Service>();
builder.Services.AddScoped<ITenant_License_Repo, Tenant_License_Repo>();
builder.Services.AddScoped<ITenant_License_Keys_Service, Tenant_License_Keys_Service>();
builder.Services.AddScoped<IExcell_Import_Service, Excell_Import_Service>();
builder.Services.AddScoped<IBulk_User_Import_Service, Bulk_User_Import_Service>();
builder.Services.AddScoped<IUser_Batch_Process_Repo, User_Batch_Process_Repo>();
builder.Services.AddScoped<IUser_Batch_Process_Service, User_Batch_Process_Service>();
builder.Services.AddScoped<IReports_Appple_Passes_Service, Reports_Appple_Passes_Service>();
builder.Services.AddScoped<IWallet_Pass_Service, Wallet_Pass_Service>();
builder.Services.AddScoped<IWallet_Passes_Repo, Wallet_Passes_Repo>();
builder.Services.AddScoped<IValidate_Txn_Service, Validate_Txn_Service>();
builder.Services.AddScoped<IPass_Transmission_Repo, Pass_Transmission_Repo>();
builder.Services.AddScoped<ISend_Pass_Customer_Service,Send_Pass_Customer_Service>();
builder.Services.AddScoped<IAccount_Balance_Repo, Account_Balance_Repo>();
builder.Services.AddScoped<IAccount_Balance_Service, Account_Balance_Service>();
builder.Services.AddScoped<IAccount_Transaction_Repo, Account_Transaction_Repo>();
builder.Services.AddScoped<IAccount_Transaction_Service, Account_Transaction_Service >();
builder.Services.AddScoped<IAssign_Pass_Customer_Service, Assign_Pass_Customer_Service>();
builder.Services.AddScoped<IMembership_Card_Service, Membership_Card_Service>();
builder.Services.AddScoped<IMembership_Card_Repo,Membership_Card_Repo>();

builder.Services.AddScoped<ILoyalty_Card_Repo, Loyalty_Card_Repo>();
builder.Services.AddScoped<ILoyalty_Card_Service, Loyalty_Card_Service>();

builder.Services.AddScoped<IPunch_Card_Repo, Punch_Card_Repo>();
builder.Services.AddScoped<IPunch_Card_Service, Punch_Card_Service>();



builder.Services.AddScoped<IUser_Vault_Repo, User_Vault_Repo>();
builder.Services.AddScoped<IUserVault_Service, UserVault_Service>();


builder.Services.ConfigureIdentity();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(ConfigureDTOS));
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.Configure<Certificate_Settings_Model>(builder.Configuration.GetSection("CertificateSettings"));
builder.Services.Configure<StripeModel>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ChargeService>();
builder.Services.AddScoped<Stripe.TokenService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<KeyManagementService>();

builder.Services.AddSignalR();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

//builder.Services.Configure<FormOptions>(options =>
//{
//    options.MultipartBodyLengthLimit = 3L * 1024 * 1024 * 1024;
//});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter Bearer and space with valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://arenas-pass.web.app", "http://localhost:4200", "https://arenas-pass.web.app/", "https://arenaspass.com", "https://arenascards.com", "https://arenas-cards.web.app")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app          = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
}
// Configure the HTTP request pipeline.

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();


app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseRouting();
var uploadedImagesPath = Path.Combine(builder.Environment.ContentRootPath, "UploadedImages");

if (!Directory.Exists(uploadedImagesPath))
{
    Directory.CreateDirectory(uploadedImagesPath);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadedImagesPath),
    RequestPath = "/UploadedImages"
});

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseMiddleware<TenantResolutionMiddleware>();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();




