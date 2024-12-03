using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;

namespace HealthCare.Views.Doctors
{
    public partial class Prescripcion : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/MedicalPrescriptions";

        public Prescripcion()
        {
            InitializeComponent();
            LoadPatients();
            LoadMedications();
            LoadPrescriptions();
        }

        private async void LoadPatients()
        {
            // Llamada a la API para obtener los pacientes y cargar el Picker de pacientes
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<PatientDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Patients");
                if (response != null)
                {
                    PatientPicker.ItemsSource = response;
                    PatientPicker.ItemDisplayBinding = new Binding("FullName"); // Usa el campo calculado en el DTO o `ToString()` para mostrar nombre completo
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar pacientes: {ex.Message}", "OK");
            }
        }
        private async void OnAssignPrescription(object sender, EventArgs e)
        {
            try
            {
                var roleIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(roleIdString) || !int.TryParse(roleIdString, out int roleId))
                {
                    await DisplayAlert("Error", "No se encontró el Id del doctor en el almacenamiento seguro.", "OK");
                    return;
                }

                // Obtener los valores seleccionados y crear el objeto de prescripción
                var selectedPatient = (PatientDto)PatientPicker.SelectedItem;
                var selectedMedication = (MedicationDto)MedicationPicker.SelectedItem;

                if (selectedPatient == null || selectedMedication == null)
                {
                    await DisplayAlert("Error", "Seleccione un paciente y un medicamento", "OK");
                    return;
                }

                var newPrescription = new
                {
                    PatientId = selectedPatient.Id,
                    DoctorId = roleId,  // Aseguramos que roleId sea un entero
                    IssueDate = DateTime.UtcNow.ToString("o"), // Formato ISO 8601 para la fecha
                    Description = DescriptionEntry.Text,
                    Dosage = DosageEntry.Text,
                    MedicationId = selectedMedication.Id
                };

                // Enviar la prescripción a la API
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, newPrescription);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Prescripción asignada correctamente", "OK");
                    await LoadPrescriptions(); // Recargar la lista de prescripciones
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"No se pudo asignar la prescripción: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
        private async void LoadMedications()
        {
            // Llamada a la API para obtener los medicamentos y cargar el Picker de medicamentos
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<MedicationDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Medications");
                if (response != null)
                {
                    MedicationPicker.ItemsSource = response;
                    MedicationPicker.ItemDisplayBinding = new Binding("Name"); // Usa el campo "Name" para mostrar el nombre del medicamento
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar medicamentos: {ex.Message}", "OK");
            }

        }
        private async Task LoadPrescriptions()
        {
            try
            {
                var roleIdString = await SecureStorage.GetAsync("roleId");
                if (string.IsNullOrEmpty(roleIdString) || !int.TryParse(roleIdString, out int roleId))
                {
                    await DisplayAlert("Error", "No se encontró el Id del doctor en el almacenamiento seguro.", "OK");
                    return;
                }

                // URL para obtener las prescripciones del doctor específico
                var prescriptionsUrl = $"https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/MedicalPrescriptions/ByDoctor/{roleId}";

                var prescriptions = await _httpClient.GetFromJsonAsync<List<PrescriptionDto>>(prescriptionsUrl);
                if (prescriptions != null)
                {
                    // Obtener listas de pacientes y medicamentos para completar nombres
                    var patients = await _httpClient.GetFromJsonAsync<List<PatientDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Patients");
                    var medications = await _httpClient.GetFromJsonAsync<List<MedicationDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Medications");

                    // Asignar nombres a las prescripciones
                    foreach (var prescription in prescriptions)
                    {
                        prescription.PatientName = patients?.FirstOrDefault(p => p.Id == prescription.PatientId)?.FullName;
                        prescription.MedicationName = medications?.FirstOrDefault(m => m.Id == prescription.MedicationId)?.Name;
                    }

                    PrescriptionList.ItemsSource = prescriptions;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar prescripciones: {ex.Message}", "OK");
            }
        }
        private async void OnDeletePrescription(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is PrescriptionDto prescription)
            {
                bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta prescripción?", "Sí", "No");
                if (!confirm) return;

                try
                {
                    var deleteUrl = $"{ApiUrl}/{prescription.Id}";
                    var response = await _httpClient.DeleteAsync(deleteUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Éxito", "Prescripción eliminada correctamente", "OK");
                        await LoadPrescriptions(); // Recargar la lista después de la eliminación
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar la prescripción", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }


    }

    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public override string ToString()
        {
            return FullName; // Mostrar el nombre completo en el Picker
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

        public string PatientName { get; set; } // Puedes incluir este campo en el DTO si la API lo devuelve
        public string MedicationName { get; set; } // Puedes incluir este campo en el DTO si la API lo devuelve
    }

    public class MedicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name; // Mostrar el nombre del medicamento en el Picker
        }
    }
}
