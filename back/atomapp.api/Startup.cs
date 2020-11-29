using atomapp.api.Infrastructure;
using atomapp.api.Services;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace atomapp.api
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(_configuration.GetSection(nameof(AppSettings)));

            // services
            services.AddTransient<IPythonService, PythonService>();
            services.AddTransient<IAudioConverterService, AudioConverterService>();
            services.AddTransient<ISemanticService, SemanticService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IWorkplaceService, WorkplaceService>();

            // ef core
            services.AddDbContext<AtomDBContext>();

            // asp.net
            services.AddCors();
            services.AddControllers();

            // swagger
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo { Title = "Atom App API", Version = "v0.1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
                xmlFile = $"{typeof(Startup).Assembly.GetName().Name}.xml";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // apply ef migrations on start
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AtomDBContext>();
                db.Database.Migrate();
            }

            // swagger
            app.UseSwagger(setupAction =>
            {
                setupAction.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Atom App API v0.1");
            });

            // front files
            app.UseDefaultFiles();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });
        }
    }
}
