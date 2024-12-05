using System;
using Server.Core;
using System.Linq;
using System.Text;
using Server.Domain;
using Server.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class Bulk_User_Import_Service: IBulk_User_Import_Service
    {
        private readonly ERPDb _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly UserManager<ApplicationUser> authManagerService;
        private readonly RoleManager<CustomRole> _roleManager;
        private readonly IUser_Batch_Process_Service btchService;
        public Bulk_User_Import_Service
        (
            ERPDb                            context, 
            IPasswordHasher<ApplicationUser> passwordHasher,
            UserManager<ApplicationUser> authManager,
            RoleManager<CustomRole> roleManager,
            IUser_Batch_Process_Service batch
        )
        {
            _context           = context;
            _passwordHasher    = passwordHasher;
            authManagerService = authManager;
            _roleManager       = roleManager;
            btchService        =batch;
        }


        /// <summary>
        /// Bulk User Creation
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<ResponseModel<string>> BulkInsertUsersAsync(IList<ApplicationUser> users,string Role)
        {

            ResponseModel<string> batchResponse = new ();

             var existingEmails = await _context.Users
                .Where(u => users.Select(nu => nu.Email).Contains(u.Email))
                .Select(u => u.Email.ToLower())
                .ToListAsync();

             var  uniqueUsers = users
                .Where(u => !existingEmails.Contains(u.Email.ToLower()))
                .ToList();
            if (uniqueUsers.Count ==0)
            {
                batchResponse.Status_Code = "400";
                batchResponse.Description = "No New User for Account Creation found";
                return batchResponse;
            }

            foreach (var user in uniqueUsers)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, "Asdf@1234");
            }

            _context.Users.AddRange(uniqueUsers);
            await _context.SaveChangesAsync();

            var roleExists = await _roleManager.RoleExistsAsync(Role);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new CustomRole { Name = Role, Permissions = "Read,Write,Delete,Update" });
            }
            var roles = new List<string> { Role };
            
            var roleTasks = uniqueUsers.Select(async user =>
            {
                var identityResult = await authManagerService.AddToRolesAsync(user, new List<string> { Role });
                if (identityResult.Succeeded)
                {
                    batchResponse.Status_Code = "200";
                    batchResponse.Description = "OK User account created successfully!";
                    
                }
                else
                {
                    batchResponse.Status_Code = "400";
                    batchResponse.Description = identityResult.Errors.Select(x => x.Description).FirstOrDefault();
                }
            });

            await Task.WhenAll(roleTasks);

            return batchResponse;
        }
    }
}
