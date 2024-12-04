using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HealthCare.Views.Students.Prescription
{
    public partial class PatientPrescriptionPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/MedicalPrescriptions";

        public PatientPrescriptionPage()
        {
            InitializeComponent();
            LoadPatientPrescriptions();
        }

        private async void LoadPatientPrescriptions()
        {
            try
            {
                var patientIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(patientIdString) || !int.TryParse(patientIdString, out int patientId))
                {
                    await DisplayAlert("Error", "No se encontró el Id del paciente en el almacenamiento seguro.", "OK");
                    return;
                }

                // URL para obtener las prescripciones del paciente específico
                var prescriptionsUrl = $"{ApiUrl}/ByPatient/{patientId}";

                var prescriptions = await _httpClient.GetFromJsonAsync<List<PrescriptionDto>>(prescriptionsUrl);
                if (prescriptions != null)
                {
                    foreach (var prescription in prescriptions)
                    {
                        // Cargar el nombre del doctor
                        var doctorResponse = await _httpClient.GetFromJsonAsync<DoctorDto>($"https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Doctors/{prescription.DoctorId}");
                        var medicationResponse = await _httpClient.GetFromJsonAsync<MedicationDto>($"https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Medications/{prescription.MedicationId}");
                        prescription.DoctorName = doctorResponse?.FullName;
                        prescription.MedicationName = medicationResponse?.Name;
                        
                    }

                    PrescriptionList.ItemsSource = prescriptions;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar prescripciones: {ex.Message}", "OK");
            }
        }
    }

    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime IssueDate { get; set; }
        public string Description { get; set; }
        public string Dosage { get; set; }
        public int MedicationId { get; set; }

        public string DoctorName { get; set; } // Nombre del doctor
        public string MedicationName { get; set; } // Nombre del medicamento
    }

    public class DoctorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; // Nombre completo del doctor
    }

    public class MedicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
