using Bodil.Models;

namespace Bodil.Services
{
    public interface IUserDataService
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserAsync(Guid userId);
        Task UpsertUserAsync(AppUser user);
        Task DeleteUserAsync(Guid userId);
    }
}
