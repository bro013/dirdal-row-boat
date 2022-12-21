using Bodil.Models;

namespace Bodil.Services
{
    public interface IUserService
    {
        Task LoginAsync();
        Task<AppUser> GetUserAsync();
        Task<AppUser> GetUserAsync(Guid userId);
        Task<IEnumerable<AppUser>> GetAppUsers();
        Task UpdateUserAsync(AppUser user);
    }
}
