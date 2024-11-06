using HealthCare.Views.Doctors;
using HealthCare.Views.Students;
using HealthCare.ViewModel;
using HealthCare.Models;

namespace HealthCare.Views;

public partial class UserRegisterPage : ContentPage
{
    private UserRegisterViewModel viewModel;
	public UserRegisterPage()
	{
		InitializeComponent();
        viewModel = new UserRegisterViewModel();
        BindingContext = viewModel;

	}
    private async void ToLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void NextButton(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text) ||
            UserTypePicker.SelectedItem == null)
        {
            await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
            return;
        }

        // Asigna valores al ViewModel
        viewModel.Username = UsernameEntry.Text;
        viewModel.Email = EmailEntry.Text;
        viewModel.Password = PasswordEntry.Text;
        viewModel.SelectedUserType = UserTypePicker.SelectedItem as string;

        // Llama a RegisterUser, el cual maneja la navegación
        await viewModel.RegisterUser();
    }

}
