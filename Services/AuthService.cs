﻿using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Shop.Contracts;
using Online_Shop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Online_Shop.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<(int, string)> Registeration(RegistrationModel model, string role)
        {
            role.ToLower();
            //string userRole = GetRole(role);

            //Get the roles from tables and check if it already exsists.
            IdentityRole? roleVar = await roleManager.FindByNameAsync(role);


            if (roleVar == null)
                return (0, "User role is invalid, please enter correct role");
            
            //Get the user from table and check if it already exsists.
            ApplicationUser? userExists = await userManager.FindByNameAsync(model.Username);

            if (userExists != null)
                return (0, "User already exists");

            //if not create a new one
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Name
            };

            var createUserResult = await userManager.CreateAsync(user, model.Password);

            //Failed due to any reason.
            if (!createUserResult.Succeeded)
                return (0, "User creation failed! Please check user details and try again.");

            //IF user is created
            string roleName = (roleVar.Name != null) ? roleVar.Name.ToString() : string.Empty;

            await userManager.AddToRoleAsync(user, roleName);

            return (1, "User created successfully! with role: " + roleVar.Name);
        }


        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return (0, "Invalid username");
            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Invalid password");
            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return (1, token);
        }
        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //private string GetRole(string enteredRole) => enteredRole switch
        //{
        //    "admin" => UserRoles.Admin,
        //    "user" => UserRoles.User,
        //    _ => "invalid"
        //};
    }
}

