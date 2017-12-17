using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Mc.Auth.Api;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Mc.Auth.Core.Services;
using Mc.Auth.Database.Context;
using Mc.Auth.Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using IAuthorizationService = Mc.Auth.Core.Interfaces.IAuthorizationService;

namespace Mc.Auth.Host.IoC
{
    public static class AuthModule
    {
        public static IServiceCollection AddMcAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthConfiguration(configuration)
                .AddAuthCoreModule()
                .AddAuthDatabaseModule()
                .AddAuthApiModule()
                .AddJwtAuthentication()
                .AddMvc(config =>
                {
                    config.Filters.Add(
                        new AuthorizeFilter(
                            new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build()));
                });

            return services;
        }

        private static IServiceCollection AddAuthCoreModule(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        private static IServiceCollection AddAuthApiModule(this IServiceCollection services)
        {
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<JwtSecurityTokenHandler>();
            return services;
        }

        private static IServiceCollection AddAuthDatabaseModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<ApplicationContext>();
            return services;
        }

        private static IServiceCollection AddAuthConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Authentication>(configuration.GetSection(nameof(Authentication)));
            services.AddScoped(x => x.GetService<IOptionsSnapshot<Authentication>>().Value);
            services.AddScoped(x => configuration);
            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var authSettings = services.BuildServiceProvider().GetService<Authentication>();
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
