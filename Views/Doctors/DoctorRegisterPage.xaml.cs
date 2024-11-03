namespace HealthCare.Views.Doctors;

public partial class DoctorRegisterPage : ContentPage
{
	public DoctorRegisterPage()
	{
		InitializeComponent();
	}
	private async void ToLogin(object sender, EventArgs e)
	{
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
                string.IsNullOrWhiteSpace(OfficeHoursEntry.Text) ||
                string.IsNullOrWhiteSpace(SpecialityEntry.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumberEntry.Text))
        {
            await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
            return;
        }
        else
        {
            await Navigation.PushAsync(new LoginPage());
        }
        
	}
}