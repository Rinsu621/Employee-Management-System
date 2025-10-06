using EmployeeCRUD.Api;
using EmployeeCRUD.Api.Filter;
using EmployeeCRUD.Api.Middleware;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.Services;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Seeder;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container like Issuer, Audience, SecretKey from appsetting.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddHangfire(config=>
{ 
config.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});

builder.Services.AddHangfireServer();
builder.Services.AddControllers();
//it allow access to HttpContext inside services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ProjectTeamMemberFilter>();

//Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6; //Minimum length
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

//Jwt Authentication
//Password requires 6 characters plus at least one uppercase, one lowercase, one non-alphanumeric, and one digit.Email uniqueness depends on default settings.
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

//here we can make changes, using this as usefuln for early testing


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// Authorization
builder.Services.AddAuthorization();

// Swagger configuration with JWT support, Add authorization header for entering Bearer token
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagement API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer' [space] and then your token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    };
    c.AddSecurityRequirement(securityRequirement);
});

//which websites can access resources on your server (API).
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontend",
//        policy =>
//        {
//            policy.AllowAnyOrigin()
//                  .AllowAnyMethod()
//                  .AllowAnyHeader();
//        });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:57325")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});




builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddApiDI(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

app.UseCors("AllowFrontend");

//seeding the value of seeder

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var salaryDbContext = services.GetRequiredService<SalaryDbContext>();
    await salaryDbContext.Database.MigrateAsync();

    var appDbContext = services.GetRequiredService<AppDbContext>();
    await appDbContext.Database.MigrateAsync();
    await IdentitySeeder.SeedRolesAndAdminAsync(services);

  
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard();


app.MapControllers();

app.Run();


public partial class Program { }