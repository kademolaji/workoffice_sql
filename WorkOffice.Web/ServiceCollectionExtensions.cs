using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOffice.Common.Helpers;
using WorkOffice.Common.Interfaces;
using WorkOffice.Contracts.ServicesContracts.Admin;
using WorkOffice.Contracts.ServicesContracts.Shared;
using WorkOffice.Contracts.ServicesContracts.Users;
using WorkOffice.Domain.Helpers;
using WorkOffice.Services.Admin;
using WorkOffice.Services.Email;
using WorkOffice.Services.Shared;
using WorkOffice.Services.Users;
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
            services.AddScoped<IAuditTrailService, AuditTrailService>();
            services.AddScoped<IHttpAccessorService, HttpAccessorService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IEmailJetService, EmailJetService>();
            services.AddScoped<IAdministrationService, ActivityService>();
            // Configure transcient services
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

    }
}

