using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HealthCare.Views.Students.Appointments
{
    public partial class PatientHistoryPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/appointments";

        public List<AppointmentDto> AcceptedAppointments { get; set; } = new List<AppointmentDto>();
        public List<AppointmentDto> PendingAppointments { get; set; } = new List<AppointmentDto>();

        public PatientHistoryPage()
        {
            InitializeComponent();
            LoadPatientAppointments();
        }

        private async void LoadPatientAppointments()
        {
            try
            {
                // Retrieve the roleId (patientId) from SecureStorage
                var patientIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(patientIdString) || !int.TryParse(patientIdString, out int patientId))
                {
                    await DisplayAlert("Error", "No se encontró el ID del paciente en el almacenamiento seguro.", "OK");
                    return;
                }

                // API URL to fetch appointments for the specific patient
                var appointmentsUrl = $"{ApiUrl}/ByPatient/{patientId}";

                // Fetch appointments
                var appointments = await _httpClient.GetFromJsonAsync<List<AppointmentDto>>(appointmentsUrl);
                if (appointments != null)
                {
                    // Split appointments into accepted and pending
                    AcceptedAppointments = appointments.FindAll(a => a.Status == "Accepted");
                    PendingAppointments = appointments.FindAll(a => a.Status == "Pending");

                    // Bind to the UI
                    AcceptedAppointmentsList.ItemsSource = AcceptedAppointments;
                    PendingAppointmentsList.ItemsSource = PendingAppointments;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar citas: {ex.Message}", "OK");
            }
        }
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } // Nombre del doctor, devuelto por el API
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
