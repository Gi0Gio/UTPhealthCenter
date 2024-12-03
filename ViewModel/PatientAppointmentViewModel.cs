using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace HealthCare.ViewModel
{
    public class AppointmentDto
    {
        public int id { get; set; }
        public int patientId { get; set; }
        public string patientName { get; set; }
        public string doctorName { get; set; }
        public int doctorId { get; set; }
        public string appointmentDate { get; set; } // Holds raw date string from API
        public DateTime AppointmentDate { get; set; } // Holds raw date string from API
        public string description { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public int clinicId { get; set; }

        // Parsed DateTime for binding and formatted display
        public DateTime AppointmentDateParsed => DateTime.TryParse(appointmentDate, out var parsedDate) ? parsedDate : DateTime.MinValue;
    }

    public class DoctorsDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public string FullName => $"{name} {lastName}";
    }

    public class PatientDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public string FullName => $"{name} {lastName}";
    }

    public class PatientAppointmentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AppointmentDto _newAppointment;
        public AppointmentDto NewAppointment
        {
            get => _newAppointment;
            set
            {
                _newAppointment = value;
                OnPropertyChanged(nameof(NewAppointment));
            }
        }

        private ObservableCollection<AppointmentDto> _pendingAppointments = new ObservableCollection<AppointmentDto>();
        public ObservableCollection<AppointmentDto> PendingAppointments
        {
            get => _pendingAppointments;
            set
            {
                _pendingAppointments = value;
                OnPropertyChanged(nameof(PendingAppointments));
            }
        }

        private ObservableCollection<PatientDto> _patients = new ObservableCollection<PatientDto>();
        public ObservableCollection<PatientDto> Patients
        {
            get => _patients;
            set
            {
                _patients = value;
                OnPropertyChanged(nameof(Patients));
            }
        }

        private PatientDto _selectedPatient;
        public PatientDto SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                _selectedPatient = value;
                NewAppointment.patientId = _selectedPatient?.id ?? 0;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        private ObservableCollection<DoctorsDto> _doctors = new ObservableCollection<DoctorsDto>();
        public ObservableCollection<DoctorsDto> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        private DoctorsDto _selectedDoctor;
        public DoctorsDto SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                NewAppointment.doctorId = _selectedDoctor?.id ?? 0;
                OnPropertyChanged(nameof(SelectedDoctor));
            }
        }

        private ObservableCollection<string> _appointmentTypes = new ObservableCollection<string> { "Urgente", "Especialidad", "General", "Chequeo de Rutina", "Examen de Laboratorio", "Revisión", "Psicologia"};
        public ObservableCollection<string> AppointmentTypes
        {
            get => _appointmentTypes;
            set
            {
                _appointmentTypes = value;
                OnPropertyChanged(nameof(AppointmentTypes));
            }
        }

        public PatientAppointmentViewModel()
        {
            NewAppointment = new AppointmentDto();
            LoadPatients();
            LoadDoctors();
            LoadAppointments();
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public async Task LoadAppointments()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/appointments");
                    var appointments = JsonSerializer.Deserialize<List<AppointmentDto>>(response);

                    var patientsResponse = await httpClient.GetStringAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients");
                    var patients = JsonSerializer.Deserialize<List<PatientDto>>(patientsResponse);
                    var doctorsResponse = await httpClient.GetStringAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/doctors");
                    var doctors = JsonSerializer.Deserialize<List<DoctorsDto>>(doctorsResponse);

                    PendingAppointments.Clear();

                    foreach (var appointment in appointments)
                    {
                        var patient = patients.FirstOrDefault(p => p.id == appointment.patientId);
                        if (patient != null)
                        {
                            appointment.patientName = $"{patient.name} {patient.lastName}";
                        }

                        var doctor = doctors.FirstOrDefault(d => d.id == appointment.doctorId);
                        if (doctor != null)
                        {
                            appointment.doctorName = $"{doctor.name} {doctor.lastName}";
                        }

                        if (appointment.status == "Pending")
                        {
                            PendingAppointments.Add(appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar citas: {ex.Message}", "OK");
            }
        }

        public async Task LoadPatients()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients");
                    var patients = JsonSerializer.Deserialize<List<PatientDto>>(response);

                    Patients.Clear();
                    foreach (var patient in patients)
                    {
                        Patients.Add(patient);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar pacientes: {ex.Message}", "OK");
            }
        }

        public async Task LoadDoctors()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/doctors");
                    var doctors = JsonSerializer.Deserialize<List<DoctorsDto>>(response);

                    Doctors.Clear();
                    foreach (var doctor in doctors)
                    {
                        Doctors.Add(doctor);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar doctores: {ex.Message}", "OK");
            }
        }

        public async Task SaveAppointment()
        {
            try
            {
                var appointment = new AppointmentDto
                {
                    patientId = NewAppointment.patientId,
                    doctorId = NewAppointment.doctorId,
                    appointmentDate = NewAppointment.AppointmentDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    description = NewAppointment.description,
                    type = NewAppointment.type,
                    status = "Pending",
                    clinicId = 1 // Asegura que envías el clinicId aquí
                };

                // Resto del código para la solicitud HTTP
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al guardar cita: {ex.Message}", "OK");
            }
        }


        public async Task UpdateAppointment()
        {
            try
            {
                var appointment = new
                {
                    doctorId = NewAppointment.doctorId, // Asegura que este valor se envíe correctamente
                    status = "Accepted" // Cambia el estado según sea necesario
                };

                using (var httpClient = new HttpClient())
                {
                    var json = JsonSerializer.Serialize(appointment);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync($"https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/appointments/clinicUpdate?appointmentId={NewAppointment.id}&status=Accepted&doctorId={NewAppointment.doctorId}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Cita actualizada correctamente", "OK");
                        await LoadAppointments(); // Recarga la lista de citas
                        NewAppointment = new AppointmentDto(); // Reinicia el formulario
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo actualizar la cita: {errorMessage}", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al actualizar cita: {ex.Message}", "OK");
            }
        }



        public async Task DeleteAppointment(AppointmentDto appointment)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync($"https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/appointments/{appointment.id}");

                    if (response.IsSuccessStatusCode)
                    {
                        PendingAppointments.Remove(appointment);
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Cita eliminada correctamente", "OK");
                    }
                    else
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo eliminar la cita: {errorMessage}", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar cita: {ex.Message}", "OK");
            }
        }
    }
}
