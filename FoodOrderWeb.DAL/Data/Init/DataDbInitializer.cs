using FoodOrderWeb.Core.DataBase;
// Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace FoodOrderWeb.DAL.Data.Init
{
    public class DataDbInitializer
    {
        public async Task SeedAsync(IApplicationBuilder app)
        {
            using (var score = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = score.ServiceProvider.GetRequiredService<UserManager<User>>();

                var roleManager = score.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    Role roleAdmin = new Role("Admin");
                    await roleManager.CreateAsync(roleAdmin);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    Role roleUser = new Role("User");
                    await roleManager.CreateAsync(roleUser);
                }
                
                User admin  = await  userManager.FindByNameAsync("Admin");
                if (admin == null)
                {
                    var user = new User
                    {
                        UserName = "support@megasoft.kg",
                        Email = "support@megasoft.kg",
                        FirstName = "Admin",
                        LastName = "Admin",
                        PatronymicName = "Admin",
                        FullName = "Admin",
                        DateOfBirth = DateTime.Today,
                        Sex = "male",
                        IsBloked = false,
                        //ImagePath = "images/admin.png"
                    };
                    var result = await userManager.CreateAsync(user, "123456");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
    }
}