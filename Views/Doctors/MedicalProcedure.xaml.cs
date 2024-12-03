using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace HealthCare.Views.Doctors
{
    public partial class MedicalProcedure : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/MedicalRecords";

        public MedicalProcedure()
        {
            InitializeComponent();
            LoadPatients();
            LoadMedications();
            LoadProcedures();
        }

        private async void LoadPatients()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PatientDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Patients");
            PatientPicker.ItemsSource = response;
            PatientPicker.ItemDisplayBinding = new Binding("FullName");
        }

        private async void LoadMedications()
        {
            var response = await _httpClient.GetFromJsonAsync<List<MedicationDto>>("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Medications");
            MedicationPicker.ItemsSource = response;
            MedicationPicker.ItemDisplayBinding = new Binding("Name");
        }

        private async Task LoadProcedures()
        {
            var response = await _httpClient.GetFromJsonAsync<List<MedicalRecordDto>>(ApiUrl);
            if (response != null)
            {
                ProcedureList.ItemsSource = response;
            }
        }

        private async void OnSaveProcedure(object sender, EventArgs e)
        {
            var roleIdString = await SecureStorage.GetAsync("roleId");
            if (!int.TryParse(roleIdString, out int doctorId))
            {
                await DisplayAlert("Error", "No se encontró el Id del doctor en el almacenamiento seguro.", "OK");
                return;
            }

            var selectedPatient = (PatientDto)PatientPicker.SelectedItem;
            var selectedMedication = (MedicationDto)MedicationPicker.SelectedItem;

            if (selectedPatient == null || selectedMedication == null)
            {
                await DisplayAlert("Error", "Seleccione un paciente y un medicamento", "OK");
                return;
            }

            var newRecord = new
            {
                PatientId = selectedPatient.Id,
                DoctorId = doctorId,
                RecordDate = DateTime.Now,
                Diagnosis = DiagnosisEntry.Text,
                ProcedurePerformed = ProcedureEntry.Text,
                Notes = NotesEntry.Text,
                MedicationId = selectedMedication.Id
            };

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, newRecord);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Procedimiento guardado correctamente", "OK");
                await LoadProcedures();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar el procedimiento", "OK");
            }
        }

        private async void OnDeleteProcedure(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is MedicalRecordDto record)
            {
                bool confirm = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este procedimiento?", "Sí", "No");
                if (!confirm) return;

                var deleteUrl = $"{ApiUrl}/{record.Id}";
                var response = await _httpClient.DeleteAsync(deleteUrl);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Éxito", "Procedimiento eliminado correctamente", "OK");
                    await LoadProcedures();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar el procedimiento", "OK");
                }
            }
        }
        public class MedicalRecordDto
        {
            public int Id { get; set; }
            public int PatientId { get; set; }
            public int DoctorId { get; set; }
            public DateTime RecordDate { get; set; }
            public string Diagnosis { get; set; }
            public string ProcedurePerformed { get; set; }
            public string Notes { get; set; }
            public int MedicationId { get; set; }
            public string PatientName { get; set; }
            public string MedicationName { get; set; }
        }

        public class PatientDto
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName => $"{FirstName} {LastName}";
        }

        public class MedicationDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
    }
}
