namespace HealthCare.Views;

public partial class AdminRegisterPage : ContentPage
{
    public AdminRegisterPage()
    {
        InitializeComponent();
    }
    private async void ToServices(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ServicesPage());
    }
    private async void ToLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}