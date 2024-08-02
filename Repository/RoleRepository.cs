using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Contracts;
using Online_Shop.Models;

namespace Online_Shop.Repository
{
    public class RoleRepository : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager; 
        }

        public async Task<Role?> CreateAsync(Role? role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            if (!await _roleManager.RoleExistsAsync(role.Name))
                await _roleManager.CreateAsync(new IdentityRole(role.Name));

            await _context.AddAsync(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task DeleteAsync(int? roleId)
        {
            var role = await GetAsync(roleId);

            if (role == null) throw new ArgumentNullException(nameof(role));

            _context.Set<Role>().Remove(role);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllAsnyc()
        {
            return await _context.Set<Role>().ToListAsync();
        }

        public async Task<Role?> GetAsync(int? roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);

            return role;
        }

        public async Task UpdateAsync(Role? role)
        {
            if(role == null) throw new ArgumentNullException(nameof(role));

            _context.Update(role);

            await _context.SaveChangesAsync();
        }

    }
}
