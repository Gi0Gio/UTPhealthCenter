using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HealthCare.ViewModel
{
    public class UserRegisterViewModel
    {
        private readonly HttpClient _httpClient;

        public UserRegisterViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/") };
        }

        public async Task<int?> AttemptRegister(string username, string email, string password, int roleId)
        {
            var registerDto = new RegisterDto
            {
                Username = username,
                Email = email,
                Password = password,
                RoleId = roleId
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/register", registerDto);

                if (response.IsSuccessStatusCode)
                {
                    var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponseDto>();
                    return registerResponse?.UserId;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Error: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            }

            return null;
        }

        private class RegisterDto
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int RoleId { get; set; }
        }
        public class RegisterResponseDto
        {
            public int UserId { get; set; }
        }
    }
}
