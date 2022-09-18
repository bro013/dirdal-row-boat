using Bodil.Database;
using Bodil.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Bodil.Services
{
    public class UserService
    {
        private readonly ReservationContext _db;

        public UserService(ReservationContext db)
        {
            _db = db;
        }

        public async Task<User> GetTestUserAsync() => await Task.FromResult(new User()
        {
            Id = Guid.Parse("e38f3987-e112-4dc7-a024-9322855ddee1"),
            FirstName = "Bjørn",
            LastName = "Rosland",
            Email = "bjoernrosland@gmail.com",
            PhoneNumber = 99247917,
            Color = "aqua"
        });

        public async Task<IEnumerable<User>> GetUsersAsync() => await _db.Users.ToListAsync();

        public async Task<User> GetUserAsync(Guid userId) => await _db.Users.Where(user => user.Id.Equals(userId)).SingleAsync();
    }
}
