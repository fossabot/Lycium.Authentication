using Lycium.Authentication;
using LyciumFreesqlClient.Request;
using LyciumFreesqlClient.Services;
using LyciumFreesqlServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace LyciumFreesqlClient
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

            services
                .AddControllers()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddHttpClient();
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, SwaggerConfiguration.XmlName));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


            services.AddLyciumAuthentication(item => item
                .SetServerUrl("https://localhost:8001")
                .SetSecretKey("fd4685a2-b78f-4dda-9d59-5d420410cf96"));

            services.AddSingleton(p => new FreeSql.FreeSqlBuilder()
                                                            .UseConnectionString(FreeSql.DataType.PostgreSQL,"")
                                                            //.UseAutoSyncStructure(true) //自动迁移实体的结构到数据库
                                                            .Build());

            services.AddScoped<LyciumRequest>();
            services.AddScoped<IClientHostService, FreesqlClientHostService>();
            services.AddScoped<IClientResourceService, FreesqlClientResourceService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAuthenticationDelegate @delegate)
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

            app.UseLyciumAuthentication(@delegate);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
