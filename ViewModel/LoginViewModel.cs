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

        public async Task<bool> PerformLoginAsync(string username, string password)
        {
            var loginDto = new { UsernameOrEmail = username, Password = password };

            try
            {
                // Realizar la solicitud POST a la API de autenticación
                var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    // Guardar la sesión del usuario en SecureStorage
                    if (loginResponse != null)
                    {
                        await SecureStorage.SetAsync("UserId", loginResponse.UserId.ToString());
                        await SecureStorage.SetAsync("RoleId", loginResponse.RoleId.ToString());
                        //await SecureStorage.SetAsync("UserRole", loginResponse.Role);

                        return true;
                    }
                }
                else
                {
                    
                    await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos.", "OK");
                }
            }
            catch (Exception ex)
            {
                
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }

            return false;
        }

        public class LoginResponse
        {
            public int UserId { get; set; }
            public int RoleId { get; set; }
            //public string Role { get; set; }
        }
    }
}
