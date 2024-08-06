using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Online_Shop.Contracts;
using Online_Shop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;

namespace Online_Shop.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        private List<String?> _roles;

        public UserRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;

            _roles = GetAllRolesFromDB();
        }

        public async Task<(int, string)> CreateUser(RegistrationModel model, Roles selectedRole)
        {
            string role = selectedRole.ToString().ToLower();
            //string userRole = GetRole(role);

            //Get the roles from tables and check if it already exsists.
            IdentityRole? roleFromDB = await roleManager.FindByNameAsync(role);

            if (roleFromDB == null)
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
            string roleName = roleFromDB.Name != null ? roleFromDB.Name.ToString() : string.Empty;

            await userManager.AddToRoleAsync(user, roleName);

            // Add claims based on Role
            (string roleClaimType, string roleClaimValue) = GetRoleClaims(selectedRole);
            await roleManager.AddClaimAsync(roleFromDB, new Claim(roleClaimType, roleClaimValue));

            // Add claims based on User
            (string userClaimType, string userClaimValue) = GetUserClaims(user);
            await userManager.AddClaimAsync(user, new Claim(userClaimType, userClaimValue));

            return (1, "User created successfully! with role: " + roleFromDB.Name);
        }

        public async Task<(int, string)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
                return (0, "Invalid username");

            if (!await userManager.CheckPasswordAsync(user, model.Password))
                return (0, "Invalid password");

            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user); // Retrieve user claims

            //Claim Variable to hold all Claims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //Add Role based claims according to Role/Roles
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                // Retrieve role claims and add them to authClaims
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        authClaims.Add(roleClaim);
                    }
                }
            }

            foreach (var claim in userClaims) // Add user claims
            {
                authClaims.Add(claim);
            }

            string token = GenerateToken(authClaims);

            return (1, $"Bearer {token}");
        }


        public async Task<(int, string)> DeleteUser(string userEmail)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
                return (0, $"User with email {userEmail} not found");

            await userManager.DeleteAsync(user);

            return (1, $"User with email {userEmail} is Deleted");
        }

        public async Task<ApplicationUser> GetUser(string userEmail)
        {
            ApplicationUser? userById = await userManager.FindByEmailAsync(userEmail);

            if (userById == null) throw new NullReferenceException($"user not found with email: {userEmail}");

            return userById;
        }

        public async Task<List<ApplicationUser>> GetAllUser()
        {
            var allUsers = await userManager.Users.ToListAsync();

            if (allUsers == null)
                throw new NullReferenceException($"users not found");

            return allUsers;
        }

        public async Task<(int, string)> UpdateUser(RegistrationModel model, string newPassword)
        {
            ApplicationUser? exsistingUser = await userManager.FindByEmailAsync(model.Email);

            if (exsistingUser == null)
                return (0, $"User does not exsists with name {model.Email}");

            string correctPasswordHash = string.Empty;

            if (exsistingUser.PasswordHash is not null)
                correctPasswordHash = exsistingUser.PasswordHash;
            else
                return (0, $"Password hash for {model.Email} not found");


            // Create an instance of PasswordHasher
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            // Verify the entered password against the stored hash
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(exsistingUser, correctPasswordHash, model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return (0, "Incorrect password entered for " + model.Email);
            }

            exsistingUser.UserName = model.Username;
            exsistingUser.Name = model.Name;

            try
            {
                await userManager.UpdateAsync(exsistingUser);

                // Change the password
                var changePasswordResult = await userManager.ChangePasswordAsync(exsistingUser, model.Password, newPassword);

                if (!changePasswordResult.Succeeded)
                {
                    return (0, "Password change failed: " + string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)));
                }

                return (1, $"User successfully Updated ! with email: {model.Email} ");
            }
            catch (Exception ex)
            {
                return (0, "User updation failed " + ex);
            }
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


        private string GetRole(string enteredRole) => enteredRole switch
        {
            "admin" => UserRoles.Admin,
            "user" => UserRoles.User,
            _ => "invalid"
        };

        //Tupple pattern
        private (string, string) GetRoleClaims(Roles role) => role switch
        {
            Roles.Admin => ("Claim_Admin", "Value_Admin"),
            Roles.Manager => ("Claim_Manager", "Claim_Manager"),
            Roles.Member => ("Claim_Member", "Claim_Member"),
            _ => throw new ArgumentOutOfRangeException(nameof(role), $"Not expected role value: {role}")
        };

        private (string, string) GetUserClaims(ApplicationUser user)
        {
            string roleClaimType = $"Claim_{user.UserName}";
            string roleClaimValue = $"Value_{user.UserName}";

            return (roleClaimType, roleClaimValue);
        }

        private List<String?> GetAllRolesFromDB()
        {
            List<string?> roles = roleManager.Roles.Select(x => x.Name).ToList();

            for (int i = 0; i < roles.Count; i++)
                roles[i] = roles[i]?.ToLower();

            return (roles.Count > 0) ? roles : throw new NullReferenceException();
        }

    }
}

public enum Roles
{
    [EnumMember(Value = "Admin")]
    Admin,
    [EnumMember(Value = "Manager")]
    Manager,
    [EnumMember(Value = "Member")]
    Member

}




