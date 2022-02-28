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
using Test.BusinessLogic;
using Test.Data.DataContext;
using Test.DataAccess;
using Test.Interfaces.Repositories;
using Test.Interfaces.Services    ;

namespace TestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public ServiceCollection Services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddSingleton<IConnection, Connection>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<IActivityRepository, ActivityRepostirory>();
            services.AddScoped<IActivityServices , ActivityService>();
            //Dem�s c�digo debe estar antes de app.UseRouting, app.useMVc
            services.AddCors();
            //Se agrega en generador de Swagger
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var urlAceptadas = Configuration
                       .GetSection("AllowedHosts").Value.Split(",");
            app.UseCors(builder => builder.WithOrigins(urlAceptadas)
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_IA V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
