﻿using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize() 
        {
            //migrations if they are not applied

            try 
            {
               if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                } 
            }
            catch (Exception ex) 
            {
                
            }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company))
                    .GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "adminOfficial@gmail.com",
                    Email = "adminOfficial@gmail.com",
                    Name = "Patrick Dumoulin",
                    PhoneNumber = "555-777-8888",
                    StreetAddress = "9160 avenue du peuplier",
                    State = "IL",
                    PostalCode = "G6J1N3",
                    City = "Chicago"
                }, "@Aa123456").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.
                    FirstOrDefault(u => u.Email == "adminOfficial@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            
            
        }
    }
}
