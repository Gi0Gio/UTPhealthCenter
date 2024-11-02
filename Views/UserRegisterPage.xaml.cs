using HealthCare.Views.Doctors;
using HealthCare.Views.Students;

namespace HealthCare.Views;

public partial class UserRegisterPage : ContentPage
{
	public UserRegisterPage()
	{
		InitializeComponent();
	}


    private async void ToLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void NextButton(object sender, EventArgs e)
    {
        // Verifica el tipo de usuario seleccionado en el Picker
        string selectedUserType = UserTypePicker.SelectedItem as string;

        if (selectedUserType == "Doctor")
        {
            // Navega a la página de registro de doctor
            await Navigation.PushAsync(new DoctorRegisterPage());
        }
        else if (selectedUserType == "Paciente")
        {
            // Navega a la página de registro de paciente
            await Navigation.PushAsync(new PatientRegisterPage());
        }
        else
        {
            // Muestra un mensaje si no se selecciona ningún tipo de usuario
            await DisplayAlert("Error", "Por favor, seleccione el tipo de usuario.", "OK");
        }
    }
}
