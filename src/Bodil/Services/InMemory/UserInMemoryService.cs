using Bodil.Models;

namespace Bodil.Services.InMemory
{
    public class UserInMemoryService : IUserDataService
    {
        private List<AppUser> _users;

        public UserInMemoryService()
        {
            _users = new List<AppUser>()
            {
                new AppUser()
                {
                    Id = Guid.Parse("fe108034-9d68-4673-9d6b-c89ebc94a7d0"),
                    FirstName = "Bjørn",
                    LastName = "Rosland",
                    Color = "red",
                    PhoneNumber = "99247917",
                    Email = "bjoernrosland@gmail.com"
                },
                new AppUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Color = "blue",
                    PhoneNumber = "33237919",
                    Email = "jane.doe@gmail.com"
                },
                new AppUser()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "¨John",
                    LastName = "Doe",
                    Color = "green",
                    PhoneNumber = "33234915",
                    Email = "john.doe@gmail.com"
                }
        };
            
        }
        public Task DeleteUserAsync(Guid userId)
        {
            var user = _users.Find(u => u.Id == userId);
            if (user is not null) _users.Remove(user);
            return Task.CompletedTask;
        }

        public Task<AppUser> GetUserAsync(Guid userId)
        {
            var user = _users.Find(u => u.Id == userId);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task UpsertUserAsync(AppUser user)
        {
            var deleteUser = _users.Find(u => u.Id == user.Id);
            if (deleteUser is not null)
                _users.Remove(deleteUser);
            _users.Add(user);
            return Task.CompletedTask;
        }
    }
}
