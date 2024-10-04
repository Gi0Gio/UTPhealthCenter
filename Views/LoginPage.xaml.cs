namespace HealthCare.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }
    private async void ToServices(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ServicesPage());
    }
    private async void AdminRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminRegisterPage());
    }
}