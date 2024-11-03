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
            await Navigation.PushAsync(new ServicesPage());
        }
    }
    private async void UserRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserRegisterPage());
    }
}