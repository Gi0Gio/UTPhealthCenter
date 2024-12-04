using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HealthCare.Views.Doctors.Citas;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HealthCare.Views.Doctors.Citas
{
    public partial class TusCitas : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/appointments";

        public List<AppointmentDto> AcceptedAppointments { get; set; } = new List<AppointmentDto>();
        public List<AppointmentDto> PendingAppointments { get; set; } = new List<AppointmentDto>();

        public TusCitas()
        {
            InitializeComponent();
            LoadDoctorAppointments();
        }

        private async void LoadDoctorAppointments()
        {
            try
            {
                // Retrieve the roleId (doctorId) from SecureStorage
                var doctorIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(doctorIdString) || !int.TryParse(doctorIdString, out int doctorId))
                {
                    await DisplayAlert("Error", "No se encontró el ID del doctor en el almacenamiento seguro.", "OK");
                    return;
                }

                // API URL to fetch appointments for the specific doctor
                var appointmentsUrl = $"{ApiUrl}/ByDoctor/{doctorId}";

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

        private async void OnViewDetails(object sender, EventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.CommandParameter as AppointmentDto;

            if (appointment != null)
            {
                await DisplayAlert("Detalles de la Cita",
                    $"Paciente: {appointment.PatientId}\n" +
                    $"Fecha: {appointment.AppointmentDate:dd/MM/yyyy HH:mm}\n" +
                    $"Descripción: {appointment.Description}\n" +
                    $"Tipo: {appointment.Type}\n" +
                    $"Estado: {appointment.Status}",
                    "OK");
            }
        }

        private async void OnMarkAsCompleted(object sender, EventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.CommandParameter as AppointmentDto;

            if (appointment != null)
            {
                bool isConfirmed = await DisplayAlert("Confirmar", "¿Deseas marcar esta cita como completada?", "Sí", "No");

                if (isConfirmed)
                {
                    try
                    {
                        using (var httpClient = new HttpClient())
                        {
                            var updatedAppointment = new
                            {
                                Status = "Completed"
                            };

                            var json = System.Text.Json.JsonSerializer.Serialize(updatedAppointment);
                            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                            var response = await httpClient.PutAsync($"{ApiUrl}/{appointment.Id}", content);

                            if (response.IsSuccessStatusCode)
                            {
                                appointment.Status = "Completed";
                                await DisplayAlert("Éxito", "Cita marcada como completada", "OK");
                                LoadDoctorAppointments(); // Reload the updated appointments list
                            }
                            else
                            {
                                await DisplayAlert("Error", "No se pudo marcar la cita como completada.", "OK");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", $"Error al marcar como completada: {ex.Message}", "OK");
                    }
                }
            }
        }
    }

    public class AppointmentDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } // Nombre del paciente, devuelto por el API
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
