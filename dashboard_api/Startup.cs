using dashboard_api.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace dashboard_api
{
    public class Startup
    {
        readonly string minhaPolitica = "_minhaPolitica";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy(name: minhaPolitica,
                    builder =>
                    {
                        builder.WithOrigins("*")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            // fim

            
            // Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Dashboard API.",
                        Version = "v1",
                        Description = "Bem vindo à API do Dashboard"
                    }
                );
            });
            // fim

            services.AddControllers();

            services.AddScoped<DbSession>();
            services.AddTransient<IPedidoRepository, PedidoRepository>();
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

            app.UseAuthentication();

            app.UseCors(minhaPolitica);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = string.Empty;
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Dashboard API");
            });
            // fim
        }
    }
}
