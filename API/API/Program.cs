
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
using System.Reflection;
using Server.Middlewares;
using API.TenantMiddleware;
using API.ExtensionMethods;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
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
builder.Services.AddScoped<PaymentIntentConfirmOptions>();
builder.Services.AddScoped<PaymentIntentService>();
builder.Services.AddScoped<IUnit_Of_Work_Repo, Unit_Of_Work_Repo>();


var servicesAssembly   = typeof(ILog_Service).Assembly;  
var repositoryAssembly = typeof(ILog_Repo).Assembly;     

builder.Services.AddRepositoriesAndServices(servicesAssembly, repositoryAssembly);

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




