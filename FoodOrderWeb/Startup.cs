using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodOrderWeb;
using FoodOrderWeb.Core.DataBase;
using FoodOrderWeb.DAL.Data;
using FoodOrderWeb.DAL.Data.Contracts;
using FoodOrderWeb.DAL.Unit;
using FoodOrderWeb.DAL.Unit.Contracts;
using FoodOrderWeb.Service.Services;
using FoodOrderWeb.Service.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FoodOrderWeb.DAL.Data.Init;
using Microsoft.AspNetCore.Identity;

namespace FoodEstablishment.Web
{
    public class Startup
    {
        private string _contentRootPath = "";

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _contentRootPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connection = Configuration.GetConnectionString("DefaultConnection");
            if (connection.Contains("%CONTENTROOTPATH%"))
            {
                connection = connection.Replace("%CONTENTROOTPATH%", _contentRootPath);
            }

            services.AddDbContext<DataDbContext>(options => options.UseSqlServer(connection));
            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            var optionsBuilder = new DbContextOptionsBuilder<DataDbContext>();
            optionsBuilder.UseSqlServer(connection);
            services.AddSingleton<IDataDbContextFactory>(
                sp => new DataDbContextFactory(optionsBuilder.Options));

            // services.AddDbContext<IDataDbContextFactory>(options => options.UseSqlServer(connection));

            /*services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataDbContext>()
                .AddDefaultTokenProviders();*/
            services.AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 3;
                    //options.Password.RequiredUniqueChars = 1;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<DataDbContext>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IBasketService, BasketService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Mapper.Initialize(config => config.AddProfile(new MappingProfile()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            new DataDbInitializer().SeedAsync(app).GetAwaiter();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
