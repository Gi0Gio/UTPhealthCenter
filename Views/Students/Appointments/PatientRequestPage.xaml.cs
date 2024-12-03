using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Maui.Storage;

namespace HealthCare.Views.Students.Appointments
{
    public partial class PatientRequestPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Appointments/patientCreate";

        public PatientRequestPage()
        {
            InitializeComponent();
        }

        private async void OnRequestAppointmentClicked(object sender, EventArgs e)
        {
            try
            {
                // Retrieve patient ID from SecureStorage
                var patientId = await SecureStorage.GetAsync("RoleId");
                if (string.IsNullOrEmpty(patientId))
                {
                    await DisplayAlert("Error", "Patient ID not found", "OK");
                    return;
                }

                // Create appointment DTO
                var appointmentDto = new
                {
                    PatientId = int.Parse(patientId),
                    AppointmentDate = AppointmentDatePicker.Date,
                    Type = TypeEntry.SelectedItem?.ToString(),
                    Description = DescriptionEditor.Text,
                    ClinicId = 1,  // Assuming default ClinicId
                    Status = "Pending"
                };

                // Serialize DTO
                var content = new StringContent(JsonConvert.SerializeObject(appointmentDto), Encoding.UTF8, "application/json");

                // Send request
                var response = await _httpClient.PostAsync($"{ApiUrl}?patientId={patientId}", content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Appointment requested successfully", "OK");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Failed to request appointment: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
