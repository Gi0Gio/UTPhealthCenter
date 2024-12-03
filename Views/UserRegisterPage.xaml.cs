using HealthCare.ViewModel;
using HealthCare.Views.Doctors;
using HealthCare.Views.Students;

namespace HealthCare.Views;

public partial class UserRegisterPage : ContentPage
{
    private readonly HttpClient _httpClient =new HttpClient();
    private const string ApiUrl = "https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/Users/register";
    public UserRegisterPage()
    {
        InitializeComponent();
    }
    private async void ToLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var roleIdEntry = 0;
        if (UserTypePicker.SelectedItem?.ToString() == "Doctor")
        {
            roleIdEntry = 2;
        }
        else if (UserTypePicker.SelectedItem?.ToString() == "Paciente")
        {
            roleIdEntry = 3;
        }
        else
        {
            await DisplayAlert("Error", "Rol no encontrado", "OK");
        }
        var userRegisterViewModel = new UserRegisterViewModel();
        var registerSuccessful = await userRegisterViewModel.AttemptRegister(UsernameEntry.Text, EmailEntry.Text, PasswordEntry.Text, roleIdEntry);
        if (registerSuccessful.HasValue)
        {
            await DisplayAlert("Registro exitoso", "Tu cuenta ha sido creada exitosamente", "OK");
            if(roleIdEntry == 2)
            {
                await Navigation.PushAsync(new DoctorRegisterPage(registerSuccessful.Value));
            }
            else if (roleIdEntry == 3)
            {
                await Navigation.PushAsync(new PatientRegisterPage(registerSuccessful.Value));
            }
        }
        else
        {
            await DisplayAlert("Error", "No se pudo crear tu cuenta", "OK");
        }
    }
}
