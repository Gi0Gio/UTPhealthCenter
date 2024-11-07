using HealthCare.Views;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace HealthCare.ViewModel
{
    public class PatientRegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly int _userId;

        private string _firstName;
        private string _lastName;
        private string _dni;
        private string _address;
        private DateTime _birthDate;
        private string _phoneNumber;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string Dni
        {
            get => _dni;
            set
            {
                _dni = value;
                OnPropertyChanged(nameof(Dni));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterPatient());

        public PatientRegisterViewModel(int userId)
        {
            _userId = userId;
            BirthDate = DateTime.Today; // Inicialización para evitar errores con DateTime predeterminado
        }

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public async Task RegisterPatient()
        {
            var patientDto = new
            {
                UserId = _userId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Dni = this.Dni,
                Address = this.Address,
                BirthDate = this.BirthDate.ToString("yyyy-MM-dd"), // Formato adecuado
                PhoneNumber = this.PhoneNumber
            };

            var jsonContent = JsonSerializer.Serialize(patientDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/patients", content);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Registro exitoso", "El paciente ha sido registrado exitosamente.", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un problema al registrar al paciente: {errorMessage}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}
