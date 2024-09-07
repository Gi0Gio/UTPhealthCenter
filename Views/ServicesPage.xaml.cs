namespace HealthCare.Views;

public partial class ServicesPage : ContentPage
{
	public ServicesPage()
	{
		InitializeComponent();
	}

	//Manejo del evento tapped 
	private async void Login(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new LoginPage());
	}
}