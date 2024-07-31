using Online_Shop.Models;

namespace Online_Shop.Contracts
{
    public interface IRoleService
    {
        Task<Role?> GetAsync(int? roleId);

        Task<List<Role>> GetAllAsnyc();

        Task<Role?> CreateAsync(Role? role);

        Task<Role?> DeleteAsync(int? roleId);

        Task<Role?> UpdateAsync(Role? role);
    }
}
