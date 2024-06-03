using AddressBook.API.Exceptions;
using AddressBook.Application.DTOs.JWT;
using AddressBook.Application.Interfaces;
using AddressBook.Application.Interfaces.Authentication;
using AddressBook.Application.Interfaces.Dashboard;
using AddressBook.Application.Interfaces.Users;
using AddressBook.Application.Mapper.Configuration;
using AddressBook.Application.Repositories;
using AddressBook.Application.Services.Authentication;
using AddressBook.Application.Services.Dashboard;
using AddressBook.Application.Validations.Authentication;
using AddressBook.Core.Entities;
using AddressBook.Core.Interfaces;
using AddressBook.Infrastructure.Data;
using AddressBook.Infrastructure.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AddressBook.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Register Auto Mapper
            builder.Services.AddAutoMapper(typeof(Program));
            AutoMapperConfiguration.Configure(builder.Services);

            // Allow CORS => Cross Origin Resource Sharing to consume my API
            builder.Services.AddCors();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("AddressBookAPI", new OpenApiInfo
                {
                    Title = "AddressBook",
                    Version = "v1",
                    Description = "AddressBook Web API Application",
                    Contact = new OpenApiContact
                    {
                        Name = "Islam Ismail",
                        Email = "islam.ismail.ali@icloud.com",
                        Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "My License",
                        Url = new Uri("https://www.linkedin.com/in/islam-ismail-ali/")
                    }
                });

                options.SwaggerDoc("AuthenticationAPIv1", new OpenApiInfo
                {
                    Title = "Authentication",
                    Version = "v1",
                    Description = "Authentication API Endpoints",
                });

                options.SwaggerDoc("AddressBookAPIv1", new OpenApiInfo
                {
                    Title = "Address Book",
                    Version = "v1",
                    Description = "Addres sBook API Endpoints",
                });

                options.SwaggerDoc("DepartmentAPIv1", new OpenApiInfo
                {
                    Title = "Departments",
                    Version = "v1",
                    Description = "Departments API Endpoints",
                });

                options.SwaggerDoc("JobAPIv1", new OpenApiInfo
                {
                    Title = "Jobs",
                    Version = "v1",
                    Description = "Jobs API Endpoints",
                });

                options.SwaggerDoc("DashboardAPIv1", new OpenApiInfo
                {
                    Title = "Dashboard",
                    Version = "v1",
                    Description = "Dashboard API Endpoints",
                });

                options.SwaggerDoc("UserAPIv1", new OpenApiInfo
                {
                    Title = "Users",
                    Version = "v1",
                    Description = "User API Endpoints",
                });

                // For Authorize the API with JWT Bearer Tokens

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT API KEY"
                });

                // For Authorize the End Points such as GET,POST 

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                options.EnableAnnotations();
            });


            // Configure the connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Fluent Validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();


            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configure password requirements
                options.Password.RequireDigit = false; // Requires a digit (0-9)
                options.Password.RequireLowercase = false; // Requires a lowercase letter (a-z)
                options.Password.RequireUppercase = false; // Requires an uppercase letter (A-Z)
                options.Password.RequireNonAlphanumeric = false; // Does not require a non-alphanumeric character
                options.Password.RequiredLength = 8; // Minimum required password length
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserManager<ApplicationUser>>();

            builder.Services.Configure<JwtDto>(builder.Configuration.GetSection("JWT"));

            // Add services UnitOfWork
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped(typeof(IDashboardService), typeof(DashboardService));
            builder.Services.AddScoped(typeof(ICommonRepository<>), typeof(CommonRepository<>));

            //Add service Account Authentication Service
            builder.Services.AddScoped(typeof(IAccountAuthenticationService), typeof(AccountAuthenticationService));

            // Add Authentication for JwtBearer Json Web Token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    //    options.InjectStylesheet("/swagger-ui/custom.css");
                    options.SwaggerEndpoint("/swagger/AuthenticationAPIv1/swagger.json", "AuthenticationAPI");
                    options.SwaggerEndpoint("/swagger/AddressBookAPIv1/swagger.json", "AddressBookAPI");
                    options.SwaggerEndpoint("/swagger/DepartmentAPIv1/swagger.json", "DepartmentAPI");
                    options.SwaggerEndpoint("/swagger/JobAPIv1/swagger.json", "JobAPI");
                    options.SwaggerEndpoint("/swagger/DashboardAPIv1/swagger.json", "DashboardAPI");
                    options.SwaggerEndpoint("/swagger/UserAPIv1/swagger.json", "UserAPI");

                });
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHttpsRedirection();

            app.UseCors(c => c.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowAnyOrigin());

            //app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // Admin Map Controller Route
                //endpoints.MapControllerRoute(
                //    name: "defualt",
                //    pattern: "api/{controller=Dashboard}/{action=Main}/{id?}");
            });

            app.Run();
        }
    }
}
