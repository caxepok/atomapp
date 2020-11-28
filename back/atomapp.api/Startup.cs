using atomapp.api.Infrastructure;
using atomapp.api.Services;
using atomapp.api.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            // asp.net
            services.AddDbContext<AtomDBContext>();
            services.AddCors();
            services.AddControllers();


        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AtomDBContext>();
                db.Database.Migrate();

            }
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
