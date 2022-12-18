using Bodil.Models;

namespace Bodil.Services
{
    public interface IUserService
    {
        Task<AppUser> GetUserAsync();
    }
}
