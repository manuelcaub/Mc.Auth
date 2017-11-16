using System;
using System.Text;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Mc.Auth.Core.Services;
using Mc.Auth.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using IAuthorizationService = Mc.Auth.Core.Interfaces.IAuthorizationService;

namespace Mc.Auth.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, false);

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration);
            services.AddScoped(cfg => cfg.GetService<IOptions<ApplicationSettings>>().Value);
            services.AddScoped(x => x.GetService<IOptions<ApplicationSettings>>().Value.Auth);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped(x => Configuration);

            services.AddDbContext<ApplicationContext>();
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
            services.AddRouting();
            services.AddMvc(config =>
            {
                config.Filters.Add(
                    new AuthorizeFilter(
                        new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
