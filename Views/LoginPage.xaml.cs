using HealthCare.Views.Students;
using HealthCare.ViewModel;
using HealthCare.Views.Doctors;

namespace HealthCare.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    private async void ToServices(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            // Muestra un mensaje de advertencia si algún campo está vacío
            await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
        }
        else
        {
            // Si ambos campos están llenos, navega a la página de servicios
            await Navigation.PushAsync(new StudentHomePage());
        }
    }
    private async void UserRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserRegisterPage());
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginViewModel = new LoginViewModel();
        var loginSuccessful = await loginViewModel.AttemptLogin(UsernameEntry.Text, PasswordEntry.Text);
        if (loginSuccessful)
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
            else
            {
                //Alerta de rol no encontrado
                await DisplayAlert("Error", "No se te ha asignado un rol", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
        }
    }
}