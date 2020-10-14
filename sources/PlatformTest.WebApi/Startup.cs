using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Core.Services;
using PlatformTest.Core.Storages;

namespace PlatformTest.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<LocalStorageOptions>(Configuration.GetSection(LocalStorageOptions.Section));

            services.AddTransient<IStorageService<FtpStorage>, FtpStorageService>();
            services.AddTransient<IStorageService<LocalStorage>, LocalStorageService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
