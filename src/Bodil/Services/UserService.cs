using Bodil.Database;
using Bodil.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Services
{
    public class UserService
    {
        private readonly IDbContextFactory<ReservationContext> _dbContextFactory;

        public UserService(IDbContextFactory<ReservationContext> contextFactory)
        {
            _dbContextFactory = contextFactory;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(Guid? userId = null)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();

            if(userId.HasValue)
                return await context.Users.Where(user => user.Id.Equals(userId)).SingleAsync();
            else
                return await context.Users.FirstAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
