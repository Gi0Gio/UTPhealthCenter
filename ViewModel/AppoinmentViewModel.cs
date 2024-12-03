using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace HealthCare.ViewModel
{
    public class AppoinmentViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Appointments/pending";

        public ObservableCollection<Appointment> PendingAppointments { get; set; } = new ObservableCollection<Appointment>();

        public event PropertyChangedEventHandler PropertyChanged;

        public AppoinmentViewModel()
        {
            LoadPendingAppointments();
        }

        private async Task LoadPendingAppointments()
        {
            var patientId = await SecureStorage.GetAsync("RoleId");
            if (string.IsNullOrEmpty(patientId))
            {
                // Handle error if patientId is not found
                return;
            }

            var response = await _httpClient.GetStringAsync($"{ApiUrl}?patientId={patientId}");
            var appointments = JsonSerializer.Deserialize<List<Appointment>>(response);

            foreach (var appointment in appointments)
            {
                PendingAppointments.Add(appointment);
            }
        }
    }

    public class Appointment
    {
        public string DoctorName { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Type { get; set; }
    }
}