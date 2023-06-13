using CustomerManagement.API.Config;
using CustomerManagement.API.Models;
using Microsoft.Extensions.Options;

namespace CustomerManagement.API.Services
{

    public interface IUsersService
    {
        public Task<List<User>> GetAllUsers();
    }
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<UsersApiOptions> _apiConfig;
        public UsersService(HttpClient httpClient, IOptions<UsersApiOptions> apiConfig)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfig;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var userResponse = await _httpClient.GetAsync(_apiConfig.Value.EndPoint);
            if(userResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<User>();
            }
            var responseContent = userResponse.Content;
            var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();
            return allUsers.ToList();
        }
    }

    
}
