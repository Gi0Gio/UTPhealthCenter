using System.Net.Http.Json;
using Microsoft.Maui.Controls;

namespace HealthCare.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/clinics";

        public ProfilePage()
        {
            InitializeComponent();
            LoadProfileAsync();
        }

        private async void LoadProfileAsync()
        {
            try
            {
                // Obtiene el ID del usuario desde el almacenamiento seguro
                var userIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(userIdString))
                {
                    await DisplayAlert("Error", "No se encontró el ID del usuario.", "OK");
                    return;
                }

                // Consulta el API para obtener los datos del usuario
                var profileUrl = $"{ApiUrl}/{userIdString}";
                var userProfile = await _httpClient.GetFromJsonAsync<UserProfileDto>(profileUrl);

                if (userProfile != null)
                {
                    // Bind los datos al BindingContext
                    BindingContext = userProfile;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron cargar los datos del perfil.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al cargar el perfil: {ex.Message}", "OK");
            }
        }
    }

    public class UserProfileDto
    {
        public string name { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string officeHours { get; set; }
    }
}
