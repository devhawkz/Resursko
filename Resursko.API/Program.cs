global using Microsoft.EntityFrameworkCore;
global using Resursko.API.Data;
global using Resursko.Domain.Models;
global using Resursko.Domain.DTOs.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Resursko.API.Services.Account;
using Resursko.API.Services.JwtHandler;
using Resursko.API.Services.EmailService;
using Resursko.API.Respositories.ResourceRespository;
using Resursko.API.Services.ResourceService;
using Resursko.API.Respositories.ReservationRespository;
using Resursko.API.Services.ReservationService;
using Resursko.API.Services.UserContext;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Resursko.API.Services.ForgotPasswotdService;
using Resursko.API.Respositories.UsersRespository;
using Resursko.API.Services.UsersServices;
namespace Resursko.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oath2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 7;
            }).AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            // configuring life span of security token (for reset password)
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromMinutes(15));

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["JwtIssuer"],
                        ValidAudience = jwtSettings["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["JwtSecurityKey"]!))
                    };
                });

            builder.Services.AddSingleton(emailConfig!);
            builder.Services.AddScoped<IEmailSenderAsync, EmailSenderAsync>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<AccountServiceHelper>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<IResourceRespository, ResourceRespository>();
            builder.Services.AddScoped<IServiceResoruce, ServiceResource>();
            builder.Services.AddScoped<IReservationRespository, ReservationRespository>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
            builder.Services.AddScoped<IUsersRespository, UsersRespository>();
            builder.Services.AddScoped<IUserServiceAPI, UserServiceAPI>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseWebAssemblyDebugging();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.Run();
        }
    }
}
