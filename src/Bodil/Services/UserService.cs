using Bodil.Models;
using Bodil.States;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace Bodil.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataService _userData;
        private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly UserState _userState;

        public UserService(IUserDataService userData,
            MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler,
            GraphServiceClient graphServiceClient,
            UserState userState)
        {
            _userData = userData;
            _consentHandler = consentHandler;
            _graphServiceClient = graphServiceClient;
            _userState = userState;
        }
        public async Task LoginAsync()
        {
            try
            {
                var user = await _graphServiceClient.Me.Request().GetAsync();
                if (Guid.TryParse(user?.Id, out var userId))
                {
                    _userState.UserId = userId;
                }
            }
            catch(Exception ex)
            {
                _consentHandler.HandleException(ex);
            }
        }
        public async Task<AppUser> GetUserAsync() => await _userData.GetUserAsync(_userState.UserId);
        public async Task<AppUser> GetUserAsync(Guid userId) => await _userData.GetUserAsync(userId);
        public async Task UpdateUserAsync(AppUser user) => await _userData.UpsertUserAsync(user);

        public async Task<IEnumerable<AppUser>> GetAppUsers() => await _userData.GetUsersAsync();
    }
}
