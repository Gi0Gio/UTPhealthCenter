using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace HealthCare.ViewModel
{
    public class LoginViewModel
    {
        private readonly HttpClient _httpClient;

        public LoginViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/") };
        }

        public async Task<bool> AttemptLogin(string usernameOrEmail, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/");

                var loginDto = new
                {
                    UsernameOrEmail = usernameOrEmail,
                    Password = password
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("auth/login", loginDto);
                if (response.IsSuccessStatusCode) {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    await SecureStorage.SetAsync("userId", loginResponse.UserId.ToString());
                    await SecureStorage.SetAsync("roleId", loginResponse.RoleId.ToString());
                    await SecureStorage.SetAsync("roleName", loginResponse.RoleName);
                    await SecureStorage.SetAsync("displayName", loginResponse.DisplayName);
                    return true;
                }
                return false;
            }
        }

        public async Task LoadUserDataAsync()
        {
            var userId = await SecureStorage.GetAsync("userId");
            var roleId = await SecureStorage.GetAsync("roleId");
            var roleName = await SecureStorage.GetAsync("roleName");
            var displayName = await SecureStorage.GetAsync("displayName");
        }

        public async Task LogoutAsync()
        {
            SecureStorage.Remove("userId");
            SecureStorage.Remove("roleId");
            SecureStorage.Remove("roleName");
            SecureStorage.Remove("displayName");

            await Task.CompletedTask;
        }
        class LoginResponse
        {
            public int UserId { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
