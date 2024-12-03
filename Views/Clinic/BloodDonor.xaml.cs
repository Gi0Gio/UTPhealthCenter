using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections.ObjectModel;

namespace HealthCare.Views.Clinic
{
    public partial class BloodDonor : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/BloodDonors";

        public ObservableCollection<BloodDonorDto> Donors { get; set; } = new ObservableCollection<BloodDonorDto>();

        public BloodDonor()
        {
            InitializeComponent();
            BindingContext = this;
            LoadBloodDonors(); // Cargar donadores al iniciar la página
        }
        private async void OnSaveDonor(object sender, EventArgs e)
        {
            var newDonor = new BloodDonorDto
            {
                Name = EntryName.Text,
                Identification = EntryIdentification.Text,
                BloodType = EntryBloodType.Text,
                LastDonationDate = DatePickerLastDonation.Date
            };

            try
            {
                var json = JsonConvert.SerializeObject(newDonor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    Donors.Add(newDonor); // Añadir el nuevo donador a la lista
                    await DisplayAlert("Success", "Donor saved successfully", "OK");

                    // Limpiar el formulario
                    EntryName.Text = string.Empty;
                    EntryIdentification.Text = string.Empty;
                    EntryBloodType.Text = string.Empty;
                    DatePickerLastDonation.Date = DateTime.Now;
                }
                else
                {
                    await DisplayAlert("Error", "Failed to save donor", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private async void LoadBloodDonors()
        {
            try
            {
                var response = await _httpClient.GetStringAsync(ApiUrl);
                var donors = JsonConvert.DeserializeObject<List<BloodDonorDto>>(response);

                Donors.Clear();
                foreach (var donor in donors)
                {
                    Donors.Add(donor);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnDeleteDonor(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is BloodDonorDto donor)
            {
                try
                {
                    var deleteResponse = await _httpClient.DeleteAsync($"{ApiUrl}/{donor.Id}");
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        Donors.Remove(donor);
                        await DisplayAlert("Success", "Donor deleted successfully", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to delete donor", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
        }
        public class BloodDonorDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Identification { get; set; }
            public string BloodType { get; set; }
            public DateTime? LastDonationDate { get; set; }
        }

    }
}
