using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthCare.ViewModel
{
    public class ClinicAppointmentViewModel
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Appointments";

        public ObservableCollection<ClinicAppointment> PendingAppointments { get; set; } = new ObservableCollection<ClinicAppointment>();

        public ICommand AcceptAppointmentCommand { get; }
        public ICommand CancelAppointmentCommand { get; }
        public ICommand AssignDoctorCommand { get; }

        public ClinicAppointmentViewModel()
        {
            AcceptAppointmentCommand = new Command<ClinicAppointment>(async appointment => await AcceptAppointment(appointment));
            CancelAppointmentCommand = new Command<ClinicAppointment>(async appointment => await CancelAppointment(appointment));
            AssignDoctorCommand = new Command<ClinicAppointment>(async appointment => await AssignDoctor(appointment));

            LoadPendingAppointments();
        }

        private async Task LoadPendingAppointments()
        {
            var response = await _httpClient.GetStringAsync($"{ApiUrl}/pending");
            var appointments = JsonSerializer.Deserialize<List<ClinicAppointment>>(response);

            foreach (var appointment in appointments)
            {
                PendingAppointments.Add(appointment);
            }
        }

        private async Task AcceptAppointment(ClinicAppointment appointment)
        {
            appointment.Status = "Accepted";
            await UpdateAppointmentStatus(appointment);
        }

        private async Task CancelAppointment(ClinicAppointment appointment)
        {
            appointment.Status = "Cancelled";
            await UpdateAppointmentStatus(appointment);
        }

        private async Task AssignDoctor(ClinicAppointment appointment)
        {
            // Replace with actual doctor assignment logic
            appointment.DoctorId = 1; // Example hardcoded doctor ID
            await UpdateAppointmentStatus(appointment);
        }

        private async Task UpdateAppointmentStatus(ClinicAppointment appointment)
        {
            var json = JsonSerializer.Serialize(appointment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{ApiUrl}/{appointment.Id}", content);
        }
    }

    public class ClinicAppointment
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string AppointmentDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int DoctorId { get; set; }
    }
}
