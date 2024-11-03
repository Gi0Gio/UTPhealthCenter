namespace HealthCare.Views.Students;

public partial class PatientRegisterPage : ContentPage
{
	public PatientRegisterPage()
	{
		InitializeComponent();
	}
    private async void Register(object sender, EventArgs e)
    {
       
        if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
            string.IsNullOrWhiteSpace(LastNameEntry.Text) ||
            string.IsNullOrWhiteSpace(DNIEntry.Text) ||
            string.IsNullOrWhiteSpace(AddressEntry.Text) ||
            string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) ||
            BirthDatePicker.Date == DateTime.Today) 
        {
           
            await DisplayAlert("Campos requeridos", "Por favor, completa todos los campos antes de continuar.", "OK");
            return;
        }

        
        await Navigation.PushAsync(new LoginPage());
    }
}