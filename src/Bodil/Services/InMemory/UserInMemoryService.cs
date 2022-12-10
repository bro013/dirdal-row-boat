using Bodil.Database.Models;

namespace Bodil.Services.InMemory
{
    public class UserInMemoryService : IUserDataService
    {
        private List<User> _users;

        public UserInMemoryService()
        {
            _users = new List<User>()
            {
                new User()
                {
                    Id = Guid.Parse("fe108034-9d68-4673-9d6b-c89ebc94a7d0"),
                    FirstName = "Bjørn",
                    LastName = "Rosland",
                    Color = "red",
                    PhoneNumber = "99247917",
                    Email = "bjoernrosland@gmail.com"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Color = "blue",
                    PhoneNumber = "33237919",
                    Email = "jane.doe@gmail.com"
                },
                new User()
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

        public Task<User> GetUserAsync(Guid userId)
        {
            var user = _users.Find(u => u.Id == userId);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task UpdateUserAsync(User user)
        {
            var deleteUser = _users.Find(u => u.Id == user.Id);
            if (deleteUser is not null)
            {
                _users.Remove(deleteUser);
                _users.Add(user);
                return Task.CompletedTask;
            }
            else
            {
                return Task.FromException(new ArgumentNullException(nameof(user)));
            }

        }
    }
}
