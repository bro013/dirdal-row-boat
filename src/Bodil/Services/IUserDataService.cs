using Bodil.Models;

namespace Bodil.Services
{
    public interface IUserDataService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid userId);
        Task UpsertUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
    }
}
