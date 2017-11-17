using Mc.Auth.Host.IoC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddAuthConfiguration(Configuration);
            services.AddAuthCoreModule();
            services.AddAuthDatabaseModule();
            services.AddAuthApiModule();
            
            services.AddJwtAuthentication();
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
