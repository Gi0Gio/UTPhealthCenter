using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace HealthCare.ViewModel
{
    public class StudentRegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private string _lastName;
        private string _dni;
        private string _location;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }

        public string DNI
        {
            get => _dni;
            set
            {
                _dni = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DNI)));
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
            }
        }

        public async Task RegisterPatient()
        {
            var patient = new
            {
                name = Name,
                lastName = LastName,
                dni = DNI,
                location = Location
            };

            using (var httpClient = new HttpClient())
            {
                var json = JsonSerializer.Serialize(patient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://giowebtestapinstance.azurewebsites.net/api/Patients", content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful registration
                    await Application.Current.MainPage.DisplayAlert("Success", "Patient registered successfully", "OK");
                }
                else
                {
                    // Handle error
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to register patient", "OK");
                }
            }
        }
    }

}
