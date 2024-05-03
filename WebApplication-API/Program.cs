using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication_API;
using WebApplication_API.DapperRepository;
using WebApplication_API.Domains;
using WebApplication_API.DTO;
using WebApplication_API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<BlobConfigs>(builder.Configuration.GetSection("BlobConfigs"));
//const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          policy.WithOrigins("http://test.com",
//                                              "https://test.com", "https://localhost");
//                      });
//});
#region auth
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<SaleContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ClockSkew = new TimeSpan(0, 5, 0),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();
#endregion

#region repository
var _connection = builder.Configuration.GetConnectionString("SqlServerSaleDatabaseConnectionString");
var _SQLiteConnection = builder.Configuration.GetConnectionString("SQLiteSaleDatabaseConnectionString");
switch ((builder.Configuration.GetValue<string>("DatabaseType") ?? "").ToUpper())
{
    case "SQLSERVER":
        builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(_connection));
        builder.Services.AddDbContext<SaleContext>(options => options.UseSqlServer(
        _connection, b =>
        {
            _ = b.MigrationsAssembly("SqlServerMigrations");
            _ = b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }));
        break;
    case "SQLITE":
        builder.Services.AddTransient<IDbConnection>((sp) => new SqliteConnection(_SQLiteConnection));
        builder.Services.AddDbContext<SaleContext>(options => options.UseSqlite(
       _SQLiteConnection, b =>
       {
           _ = b.MigrationsAssembly("SQLiteMigrations");
           _ = b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
       }));
        break;
}
#endregion

#region Dependency injection definition
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IGetPluralName, GetPluralName>();
builder.Services.AddScoped<IInvoiceServices, InvoiceServices>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceReportService, InvoiceReportService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
#endregion


#region permission
builder.Services.AddAuthorizationBuilder().RegisterPolicies();
#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TestMehdi", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", [AllowAnonymous] () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.ExcludeFromDescription().AllowAnonymous();

app.MapGroup("/Invoice").MapApiEndpoints();

app.MapGroup("/auth/v1/").MapAuthApiV1().WithTags("Authentication Apis");
app.Run();

//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{

    public int TemperatureF
    {
        get
        {
            return 32 + (int)(TemperatureC / 0.5556);
        }
    }
}

