using Azure;
using Azure.Data.Tables;
using Bodil.Models;

namespace Bodil.Services.TableStorage
{
    public class UserTableStorage : IUserDataService
    {
        private readonly TableClient _tableClient;
        public UserTableStorage(TableClientFactory tableClientFactory)
        {
            _tableClient = tableClientFactory.GetTableClient("users");
            _tableClient.CreateIfNotExists();
        }
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserAsync(userId);
            await _tableClient.DeleteEntityAsync(user.PartitionKey, user.RowKey, ETag.All);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var resultPages = _tableClient.QueryAsync<User>(user => user.Id.Equals(userId)).GetAsyncEnumerator();
            await resultPages.MoveNextAsync();
            return resultPages.Current;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = new List<User>();
            await foreach (var user in _tableClient.QueryAsync<User>()) users.Add(user);
            return users;
        }

        public async Task UpsertUserAsync(User user) =>
            await _tableClient.UpsertEntityAsync(user);
    }
}
