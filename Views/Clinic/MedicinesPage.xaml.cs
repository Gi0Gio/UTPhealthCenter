using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace HealthCare.Views
{
    public partial class MedicinesPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Medications";

        public ObservableCollection<MedicationDto> Medications { get; set; } = new ObservableCollection<MedicationDto>();

        public MedicinesPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadMedications();
        }

        private void ShowMedicineList(object sender, EventArgs e)
        {
            //MedicineListSection.IsVisible = true;
            AssignMedicineSection.IsVisible = false;
            MedicineFormSection.IsVisible = false;
        }

        private void ShowAssignMedicine(object sender, EventArgs e)
        {
            //MedicineListSection.IsVisible = false;
            AssignMedicineSection.IsVisible = true;
            MedicineFormSection.IsVisible = false;
        }

        private void ShowMedicineForm(object sender, EventArgs e)
        {
            //MedicineListSection.IsVisible = false;
            AssignMedicineSection.IsVisible = false;
            MedicineFormSection.IsVisible = true;
        }

        private async void OnSaveMedicine(object sender, EventArgs e)
        {
            try
            {
                // Medicine DTO
                var medicineDto = new
                {
                    Name = EntryNombre.Text,
                    Description = EntryDescription.Text,
                    Quantity = int.TryParse(EntryQuantity.Text, out int quantity) ? quantity : 0,
                    ExpirationDate = EntryExpirationDate.Date,
                    RecommendedDosage = EntryDosis.Text
                };

                // Serialize DTO
                var content = new StringContent(JsonConvert.SerializeObject(medicineDto), Encoding.UTF8, "application/json");

                // Send request to the correct URL
                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Medicine created successfully", "OK");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Failed to create medicine: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private async void LoadMedications()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ApiUrl);
                var medications = JsonConvert.DeserializeObject<List<MedicationDto>>(response);

                Medications.Clear();
                foreach (var med in medications)
                {
                    Medications.Add(med);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteMedication(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is MedicationDto medication)
            {
                try
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"{ApiUrl}/{medication.Id}");
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        Medications.Remove(medication);
                        await DisplayAlert("Success", "Medication deleted successfully", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to delete medication", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
    }
    public class MedicationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RecommendedDosage { get; set; }
    }

}
