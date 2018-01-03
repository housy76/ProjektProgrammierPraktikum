using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppData;
using AppServices;
using AppData.Models;
using System.Threading.Tasks;
using System;

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
            services.AddTransient<IAppointment, AppointmentService>();
            services.AddScoped<IAppointmentSurvey, AppointmentSurveyService>();
            services.AddTransient<IBeamer, BeamerService>();
            services.AddTransient<IBookedTime, BookedTimeService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRessource, RessourceService>();
            services.AddTransient<IRoom, RoomService>();

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


            //create roles for users
            //CreateRoles(app.ApplicationServices).Wait();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {

            IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Admin", "Manager", "Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database: Question 1
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }


            //initializing custom roles 
            //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //string[] roleNames = { "Admin", "Manager", "Member" };
            //IdentityResult roleResult;

            //foreach (var roleName in roleNames)
            //{
            //    var roleExist = await RoleManager.RoleExistsAsync(roleName);
            //    if (!roleExist)
            //    {
            //        //create the roles and seed them to the database: Question 1
            //        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            //    }
            //}

            //Here you could create a super user who will maintain the web app
            var poweruser = new ApplicationUser
            {

                UserName = Configuration["AppSettings:UserName"],
                Email = Configuration["AppSettings:UserEmail"],
            };
            //Ensure you have these values in your appsettings.json file
            string userPWD = Configuration["AppSettings:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }
    }
}
