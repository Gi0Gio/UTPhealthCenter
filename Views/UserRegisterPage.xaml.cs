using HealthCare.Views.Doctors;
using HealthCare.Views.Students;
using HealthCare.ViewModel;

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
            // Muestra un mensaje de advertencia si algún campo está vacío
            await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
            return;
        }

        // Verifica el tipo de usuario seleccionado en el Picker
        string selectedUserType = UserTypePicker.SelectedItem as string;

        if (selectedUserType == "Doctor")
        {
            
            await Navigation.PushAsync(new DoctorRegisterPage());
        }
        else if (selectedUserType == "Paciente")
        {
            
            await Navigation.PushAsync(new PatientRegisterPage());
        }
        else
        {
            
            await DisplayAlert("Error", "Por favor, seleccione el tipo de usuario.", "OK");
        }

        viewModel.Username = UsernameEntry.Text;
        viewModel.Email = EmailEntry.Text;  
        viewModel.Password = PasswordEntry.Text;
        viewModel.SelectedUserType = UserTypePicker.SelectedItem as string;

        await viewModel.RegisterUser();
    }
}
