using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace HealthCare.Views.Students
{
    public partial class PatientRegisterPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public PatientRegisterPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        public async void Register(object sender, EventArgs e)
        {
            // Validación de campos
            if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
                string.IsNullOrWhiteSpace(DNIEntry.Text) ||
                string.IsNullOrWhiteSpace(AddressEntry.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) ||
                BirthDatePicker.Date == DateTime.Today)
            {
                await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
                return;
            }

            // Crear el objeto PatientDto
            var patientDto = new
            {
                FirstName = NameEntry.Text,
                LastName = LastNameEntry.Text,
                Dni = DNIEntry.Text,
                Address = AddressEntry.Text,
                BirthDate = BirthDatePicker.Date,
                PhoneNumber = PhoneNumberEntry.Text
            };

            // Serializar el objeto a JSON
            var jsonContent = JsonSerializer.Serialize(patientDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST al endpoint de la API
            try
            {
                var response = await _httpClient.PostAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients", content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Registro exitoso", "El paciente ha sido registrado exitosamente.", "OK");
                    // Navegar a la página de inicio de sesión o a otra página si es necesario
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("Error", "Hubo un problema al registrar al paciente. Inténtalo de nuevo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}

