using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyBookList.Models;

namespace MyBookList.Helpers
{
    public class LogUserLoginInformation
    {
        public static void LogInformation(string ip, string id, UserManager<ApplicationUser> userManager)
        {
            var test = new net.webservicex.www.GeoIPService();
            var something = test.GetGeoIP("62.61.130.33");
            var user = userManager.FindById(id);
            if (user.LastLogins == null)
            {
                user.LastLogins = new List<UserLoginLog>();
            }
                
            user.LastLogins.Add(new UserLoginLog()
            {
                Ip = ip,
                CountryCode = something.CountryCode,
                CountryName = something.CountryName,
                Time = DateTime.Now
            });
            userManager.Update(user);
        }
    }
}