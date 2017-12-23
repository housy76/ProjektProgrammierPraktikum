using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppData;
using AppServices;
using AppData.Models;

namespace TerminUndRaumplanung
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
            //Simon
            services.AddMvc();
            services.AddSingleton(Configuration);
            // so that AppointmentSurveyService is injected into controllers and other components that request IAppointmentSurvey
            services.AddScoped<IAppointmentSurvey, AppointmentSurveyService>();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRessource, RessourceService>();
            services.AddTransient<IBeamer, BeamerService>();


            services.AddDbContext<AppointmentContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppointmentContext>()
                .AddDefaultTokenProviders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
