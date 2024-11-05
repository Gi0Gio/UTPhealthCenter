using System;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthCare.Views.Doctors;
using HealthCare.Views.Students;

namespace HealthCare.ViewModel
{
    public class UserRegisterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _username;
        private string _email;
        private string _password;
        private string _selectedUserType;
        private bool _isBusy;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public string SelectedUserType
        {
            get => _selectedUserType;
            set
            {
                _selectedUserType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedUserType)));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;    
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));    
            }
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterUser());

        public async Task RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(SelectedUserType))
            {
                await Application.Current.MainPage.DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                int roleId = SelectedUserType == "Doctor" ? 2 : 3; 

                var userDto = new
                {
                    Username = this.Username,
                    Email = this.Email,
                    PasswordHash = this.Password, // Asumimos que el backend se encarga del hashing
                    RoleId = roleId
                };

                var json = JsonSerializer.Serialize(userDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var httpClient = new HttpClient();
                var response = await httpClient.PostAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/users", content);

                if (response.IsSuccessStatusCode)
                {
                    // Redirige a la página correspondiente
                    if (SelectedUserType == "Doctor")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new DoctorRegisterPage());
                    }
                    else if (SelectedUserType == "Paciente")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new PatientRegisterPage());
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar el usuario. Intente nuevamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
