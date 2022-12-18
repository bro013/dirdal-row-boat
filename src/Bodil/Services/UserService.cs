using Bodil.Models;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http;

namespace Bodil.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataService _userData;
        private readonly ITokenAcquisition _tokenAcquisitionService;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IUserDataService userData, ITokenAcquisition tokenAcquisitionService, IHttpClientFactory httpClientFactory)
        {
            _userData = userData;
            _tokenAcquisitionService = tokenAcquisitionService;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<AppUser> GetUserAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "Mail.Read" });

            // make API call
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var dataRequest = await httpClient.GetAsync("https://graph.microsoft.com/beta/me");
            if (dataRequest.IsSuccessStatusCode)
            {
                var userData = System.Text.Json.JsonDocument.Parse(await dataRequest.Content.ReadAsStreamAsync());
                var userDisplayName = userData.RootElement.GetProperty("displayName").GetString();
            }
            return null;
        }
    }
}
