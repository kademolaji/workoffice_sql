using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Common.Helpers;
using WorkOffice.Common.Interfaces;
using WorkOffice.Contracts.ServicesContracts;
using WorkOffice.Domain.Helpers;
using WorkOffice.Services;
using WorkOffice.Web.Utilities;

namespace WorkOffice.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Configure singletons service
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerService, LoggerService>();

            // Configure DI for application services
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IHttpAccessorService, HttpAccessorService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEmailJetService, EmailJetService>();

            services.AddScoped<IStructureDefinitionService, StructureDefinitionService>();
            services.AddScoped<ICompanyStructureService, CompanyStructureService>();
            services.AddScoped<IlocationService, LocationService>();
            services.AddScoped<ICustomIdentityFormatSettingService, CustomIdentityFormatSettingService>();
            services.AddScoped<IGeneralInformationService, GeneralInformationService>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAdministrationService, AdministrationService>();
            services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

            services.AddScoped<IAppTypeService, AppTypeService>();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<INHSActivityService, NHSActivityService>();
            services.AddScoped<IPathwayStatusService, PathwayStatusService>();
            services.AddScoped<IRTTService, RTTService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IWaitingTypeService, WaitingTypeService>();
            services.AddScoped<IWardService, WardService>();
            services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();

            services.AddScoped<IPatientInformationService, PatientInformationService>();
            services.AddScoped<IPatientDocumentService, PatientDocumentService>();
            services.AddScoped<IAppointmentsServices, AppointmentsServices>();
            services.AddScoped<IWaitingListService, WaitingListService>();
            services.AddScoped<IDiagnosticService, DiagnosticService>();
            services.AddScoped<IDiagnosticResultService, DiagnosticResultService>();
            services.AddScoped<IReferralService, ReferralService>();

            services.AddScoped<DataContext>();

            return services;
        }

        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("somethinglongerforthisdumbalgorithmisrequired"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = "issuer";
            var audience = "audience";

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0].Substring("Token ".Length).Trim();
                            }

                            return Task.CompletedTask;
                        }
                    };

                });
        }

        public static void ConfigureDocumentationServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Register the Swagger generator, defining 1 or more Swagger documents
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Workoffice API",
                        Description = "Workoffice",
                        TermsOfService = new Uri("https://workoffice.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Kayode Ademolaji",
                            Email = "kademolaji@yahoo.com",
                            Url = new Uri("https://github.com.com/kademolaji"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use under LICX",
                            Url = new Uri("https://example.com/license"),
                        }
                    });
                    // Configure Swagger to use the xml documentation file
                    var xmlFile = Path.ChangeExtension(typeof(Startup).Assembly.Location, ".xml");
                    c.IncludeXmlComments(xmlFile);

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
                }
            });
        }

    }
}

