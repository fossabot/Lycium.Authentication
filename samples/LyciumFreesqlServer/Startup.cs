using Lycium.Authentication;
using Lycium.Authentication.Server;
using LyciumFreesqlServer.Request;
using LyciumFreesqlServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LyciumFreesqlServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, SwaggerConfiguration.XmlName));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });



            services.AddSingleton(p => new FreeSql.FreeSqlBuilder()
                                                            .UseConnectionString(FreeSql.DataType.PostgreSQL, "")
                                                            .UseAutoSyncStructure(true) //自动迁移实体的结构到数据库
                                                            .Build());



            services.AddScoped<IServerConfigurationService, FreeSqlServerConfigurationService>();
            services.AddScoped<IServerHostService, FreeSqlServerHostService>();
            services.AddScoped<IContextInfoService, FreesqlContextInfoService>();
            services.AddScoped<IServerResourceService, FreeSqlServerResourceService>();
            services.AddScoped<IServerHostGroupService, FreeSqlServerHostGroupService>();
            services.AddScoped<IServerTokenService, FreeSqlServerTokenService>();
            services.AddScoped<IResourceNotify, HttpResourceNotify>();
            services.AddScoped<ITokenNotify, HttpTokenNotify>();
            services.AddScoped<LyciumRequest>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";

            });

            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
