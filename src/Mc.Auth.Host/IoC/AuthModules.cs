using System;
using System.Text;
using Mc.Auth.Api;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Mc.Auth.Core.Services;
using Mc.Auth.Database.Context;
using Mc.Auth.Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mc.Auth.Host.IoC
{
    public static class AuthModule
    {
        public static IServiceCollection AddAuthCoreModule(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection AddAuthApiModule(this IServiceCollection services)
        {
            services.AddScoped<ITokenProvider, TokenProvider>();
            return services;
        }

        public static IServiceCollection AddAuthDatabaseModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<ApplicationContext>();
            return services;
        }

        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationSettings>(configuration);
            services.AddScoped(cfg => cfg.GetService<IOptions<ApplicationSettings>>().Value);
            services.AddScoped(x => x.GetService<IOptions<ApplicationSettings>>().Value.Auth);
            services.AddScoped(x => configuration);
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var authSettings = services.BuildServiceProvider().GetService<ApplicationSettings>().Auth;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = authSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
            });
            return services;
        }
    }
}
