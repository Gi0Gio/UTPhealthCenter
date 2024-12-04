using System.Net.Http.Json;

namespace HealthCare.Views.Students
{
    public partial class PatientProfilePage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients";

        public PatientProfilePage()
        {
            InitializeComponent();
            LoadProfileAsync();
        }

        private async void LoadProfileAsync()
        {
            try
            {
                // Retrieve user ID from SecureStorage
                var userIdString = await SecureStorage.GetAsync("roleId") ?? "0";
                if (userIdString == "0")
                {
                    await DisplayAlert("Error", "No se encontró el ID del usuario.", "OK");
                    return;
                }

                // Fetch patient data from API
                var profileUrl = $"{ApiUrl}/{userIdString}";
                var patientProfile = await _httpClient.GetFromJsonAsync<PatientProfileDto>(profileUrl);

                if (patientProfile != null)
                {
                    // Calculate age
                    patientProfile.BirthDateAge = $"{patientProfile.BirthDate:dd/MM/yyyy}, {CalculateAge(patientProfile.BirthDate)} años";

                    // Bind data to UI
                    BindingContext = patientProfile;
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

        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age;
        }

        public class PatientProfileDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Dni { get; set; }
            public string Address { get; set; }
            public DateTime BirthDate { get; set; }
            public string PhoneNumber { get; set; }

            public string FullName => $"{FirstName} {LastName}";
            public string BirthDateAge { get; set; }
        }
    }
}
