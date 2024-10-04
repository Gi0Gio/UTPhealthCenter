namespace HealthCare.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }
    private async void ParaServicios(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ServicesPage());
    }
    private async void ParaLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}