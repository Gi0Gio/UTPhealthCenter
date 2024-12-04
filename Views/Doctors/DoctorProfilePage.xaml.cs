using System.Net.Http.Json;

namespace HealthCare.Views.Doctors;

public partial class DoctorProfilePage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/doctors";

    public DoctorProfilePage()
	{
		InitializeComponent();
        LoadProfileAsync();

    }
    private async void LoadProfileAsync()
    {
        try
        {
            // Obtiene el ID del usuario desde el almacenamiento seguro
            var userIdString = await SecureStorage.GetAsync("roleId") ?? "0"; // Default to "0" if not found
            if (userIdString == "0")
            {
                await DisplayAlert("Error", "No se encontró el ID del usuario.", "OK");
                return;
            }

            // Consulta el API para obtener los datos del usuario
            var profileUrl = $"{ApiUrl}/{userIdString}";
            var userProfile = await _httpClient.GetFromJsonAsync<DoctorProfileDto>(profileUrl);

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
    class DoctorProfileDto
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string specialty { get; set; }
        public string phoneNumber { get; set; }
        public string officeHours { get; set; }

        public string FullName => $"{firstName} {lastName}";
    }
}