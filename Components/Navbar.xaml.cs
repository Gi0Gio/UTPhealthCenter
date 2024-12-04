using HealthCare.Views;
using HealthCare.Views.Students;
using HealthCare.Views.Clinic;
using HealthCare.Views.Doctors;
using Microsoft.Maui.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HealthCare.Components
{
    public partial class Navbar : ContentView, INotifyPropertyChanged
    {
        private string _profileName;

        public Navbar()
        {
            InitializeComponent();
            BindingContext = this;
            LoadRoleNameAsync();
        }

        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadRoleNameAsync()
        {
            var roleName = await SecureStorage.GetAsync("RoleName") ?? "Unknown Role";
            var displayName = await SecureStorage.GetAsync("DisplayName") ?? "Unknown Name";

            ProfileName = $"{roleName} {displayName}";
        }

        private async void WelcomePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WelcomePage());
        }

        private async void ServicePage(object sender, EventArgs e)
        {
            if (await SecureStorage.GetAsync("roleName") == "Patient")
            {
                await Navigation.PushAsync(new StudentHomePage());
            }
            else if (await SecureStorage.GetAsync("roleName") == "Doctor")
            {
                await Navigation.PushAsync(new DoctorHomePage());
            }
            else if (await SecureStorage.GetAsync("roleName") == "Administrator")
            {
                await Navigation.PushAsync(new ServicesPage());
            }
        }
        private async void ProfilePage(object sender, EventArgs e)
        {
            var roleName = await SecureStorage.GetAsync("roleName");

            if (roleName == "Patient")
            {
                await Navigation.PushAsync(new PatientProfilePage()); // Perfil de Paciente
            }
            else if (roleName == "Doctor")
            {
                await Navigation.PushAsync(new DoctorProfilePage()); // Perfil de Doctor
            }
            else if (roleName == "Administrator")
            {
                await Navigation.PushAsync(new ProfilePage()); // Perfil de Administrador
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
