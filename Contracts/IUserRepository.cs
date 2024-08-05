using Online_Shop.Models;

namespace Online_Shop.Contracts
{
    public interface IUserRepository
    {
        Task<(int, string)> Login(LoginModel model);
        Task<(int, string)> CreateUser(RegistrationModel model, Roles selectedRole);
        Task<(int, string)> UpdateUser(RegistrationModel model, string newPassword);
        Task<ApplicationUser> GetUser(string userEmail);
        Task<List<ApplicationUser>> GetAllUser();
        Task<(int, string)> DeleteUser(string userEmail);

    }
}
