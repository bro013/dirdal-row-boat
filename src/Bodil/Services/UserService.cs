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

        public async Task<User> GetTestUserAsync() => await Task.FromResult(new User()
        {
            Id = Guid.Parse("e38f3987-e112-4dc7-a024-9322855ddee1"),
            FirstName = "Bjørn",
            LastName = "Rosland",
            Email = "bjoernrosland@gmail.com",
            PhoneNumber = "+4799247917",
            Color = "aqua"
        });

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
    }
}
