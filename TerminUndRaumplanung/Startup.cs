using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppData;
using AppServices;
using AppData.Models;
using System.Threading.Tasks;
using System;

namespace TerminUndRaumplanung
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Simon
            services.AddMvc();
            services.AddSingleton(Configuration);
            // so that SurveyService is injected into controllers and other components that request ISurvey
            services.AddTransient<IAppointment, AppointmentService>();
            services.AddScoped<ISurvey, SurveyService>();
            services.AddTransient<IBeamer, BeamerService>();
            services.AddTransient<IBookedTime, BookedTimeService>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRessource, RessourceService>();
            services.AddTransient<IRoom, RoomService>();
            services.AddTransient<IRessourceBookedTime, RessourceBookedTimeService>();
            services.AddTransient<IAppointmentRessource, AppointmentRessourceService>();

            services.AddDbContext<AppointmentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppointmentContext>()
                .AddDefaultTokenProviders();

        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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
                //attribute routing is autmatically activated / available 
                //in asp.net core 2.0 and later. 
                //If you have to add custom routes, please write them as annotations
                //above your controller method. More information under:
                //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing#attribute-routing-ref-label

                //default route will be used when no specific or custom route matches
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                
            });


        }



        //Initialization of users and roles on startup is acutally not working
        //uncomment this section and Line 68 for further tests 
        //private async Task CreateRoles(IServiceProvider serviceProvider)
        //{

        //    IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        //    using (IServiceScope scope = scopeFactory.CreateScope())
        //    {
        //        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //        string[] roleNames = { "Admin", "Manager", "Member" };
        //        IdentityResult roleResult;

        //        foreach (var roleName in roleNames)
        //        {
        //            var roleExist = await roleManager.RoleExistsAsync(roleName);
        //            if (!roleExist)
        //            {
        //                //create the roles and seed them to the database: Question 1
        //                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        //            }
        //        }
        //    }


        //    //initializing custom roles 
        //    //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    //string[] roleNames = { "Admin", "Manager", "Member" };
        //    //IdentityResult roleResult;

        //    //foreach (var roleName in roleNames)
        //    //{
        //    //    var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //    //    if (!roleExist)
        //    //    {
        //    //        //create the roles and seed them to the database: Question 1
        //    //        roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //    //    }
        //    //}

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new ApplicationUser
        //    {

        //        UserName = Configuration["AppSettings:UserName"],
        //        Email = Configuration["AppSettings:UserEmail"],
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    string userPWD = Configuration["AppSettings:UserPassword"];
        //    var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //    if (_user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //        if (createPowerUser.Succeeded)
        //        {
        //            //here we tie the new user to the role
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}
    }
}
