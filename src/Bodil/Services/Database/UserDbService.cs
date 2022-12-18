using Bodil.Database;
using Bodil.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Services.Database
{
    public class UserDbService : IUserDataService
    {
        private readonly IDbContextFactory<ReservationContext> _dbContextFactory;

        public UserDbService(IDbContextFactory<ReservationContext> contextFactory)
        {
            _dbContextFactory = contextFactory;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserAsync(Guid userId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.Where(user => user.Id.Equals(userId)).FirstOrDefaultAsync();
        }

        public async Task UpsertUserAsync(AppUser user)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();

            if((await GetUserAsync(user.Id)) is null)
                context.Users.Update(user);
            else
                context.Users.Add(user);

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
