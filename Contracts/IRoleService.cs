using Online_Shop.Models;

namespace Online_Shop.Contracts
{
    public interface IRoleService
    {
        Task<Role?> GetAsync(int? roleId);

        Task<List<Role>> GetAllAsnyc();

        Task<Role?> CreateAsync(Role? role);

        Task DeleteAsync(int? roleId);

        Task UpdateAsync(Role? role);
    }
}
