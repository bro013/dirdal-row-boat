using Bodil.Database;
using Bodil.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Services.Database
{
    public class UserDbService : IUserService
    {
        private readonly IDbContextFactory<ReservationContext> _dbContextFactory;

        public UserDbService(IDbContextFactory<ReservationContext> contextFactory)
        {
            _dbContextFactory = contextFactory;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.Where(user => user.Id.Equals(userId)).SingleAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var user = await context.Users.Where(user => user.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user is null) return;
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
}
