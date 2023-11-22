using AspNetCoreIdentityApp.Web.Service;
using BookStoreAPI.Business.DependencyResolvers;
using BookStoreAPI.Business.Mapping;
using BookStoreAPI.Core.Utilities.EmailHelper;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//DependencyResolvers icerisindeki Servicelerimiz baska claasda yazib burada cagiririq
builder.Services.Run();


// appsettings.json icindeki Database melumatlarini Configuration edirik
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

builder.Services.AddIdentity<AppUser, AppRole>()
.AddMongoDbStores<AppUser, AppRole, Guid>(
    databaseSettings.ConnectionString, databaseSettings.UserCollectionName
);

//FluentValidator.AspNetCore elave edirik
builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

//Caching elave etmek
builder.Services.AddMemoryCache();

//SeriLog Configuration elave etmek // Information
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    //Loglari bir folder acib orada saxlayir 
    .WriteTo.File("logs/myAllLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


builder.Services.AddSingleton<IMongoCollection<AppUser>>(sp =>
{
    var client = new MongoClient(databaseSettings.ConnectionString);
    var database = client.GetDatabase(databaseSettings.DatabaseName);
    return database.GetCollection<AppUser>(databaseSettings.UserCollectionName);
});

builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nmDLKAna9f9WEKPPH7z3tgwnQ433FAtrdP5c9AmDnmuJp9rzwTPwJ9yUu")),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

var optionsOpen = new OpenApiSecurityScheme
{
    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey
};

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", optionsOpen);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//wwroot icindeki sekillere ucn yaziriq
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
