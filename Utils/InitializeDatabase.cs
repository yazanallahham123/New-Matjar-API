using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utils
{
    public class InitializeDatabase
    {
        public static async Task Run(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<MatjarDBContext>();
                var securityService = serviceScope.ServiceProvider.GetRequiredService<SecurityServices>();
                applicationDbContext.Database.Migrate();

                if (!applicationDbContext.UserRoles.Any(u => u.Id == (int)RoleType.Admin))
                {
                    UserRole userRole = new UserRole();
                    userRole.ArabicName = "مدير";
                    userRole.EnglishName = "Admin";
                    try
                    {
                        applicationDbContext.UserRoles.Add(userRole);
                        await applicationDbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (!applicationDbContext.UserRoles.Any(u => u.Id == (int)RoleType.User))
                {
                    UserRole userRole = new UserRole();
                    userRole.ArabicName = "عميل";
                    userRole.EnglishName = "Customer";
                    try
                    {
                        applicationDbContext.UserRoles.Add(userRole);
                        await applicationDbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (!applicationDbContext.Users.Any(u => u.RoleId == (int)RoleType.Admin))
                {
                    User user = new User();
                    user.Fullname = "Admin";
                    user.MobileNumber = "Admin";
                    user.RoleId = (int)RoleType.Admin;
                    user.Password = "admin1234";
                    var hashSalt = securityService.EncryptPassword(user.Password);
                    user.Password = hashSalt.Hash;
                    user.StoredSalt = hashSalt.Salt;
                    user.Token = hashSalt.Hash;

                    try
                    {
                        applicationDbContext.Users.Add(user);
                        await applicationDbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
