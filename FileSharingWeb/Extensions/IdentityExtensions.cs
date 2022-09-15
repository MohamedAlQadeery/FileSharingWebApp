using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharingWeb.Data;
using FileSharingWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace FileSharingWeb.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration config)
        {
            service.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;


            })
            .AddEntityFrameworkStores<AppDbContext>();

            service.AddAuthentication()
                    .AddFacebook(options =>
                    {
                        options.AppId = config["Auth:Facebook:AppId"];
                        options.AppSecret = config["Auth:Facebook:AppSecret"];
                    });


            return service;
        }
    }
}