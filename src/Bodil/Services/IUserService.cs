using Bodil.Models;

namespace Bodil.Services
{
    public interface IUserService
    {
        Task LoginAsync();
        Task<AppUser> GetUserAsync();
        Task<AppUser> GetUserAsync(Guid userId);
        Task UpdateUserAsync(AppUser user);
    }
}
