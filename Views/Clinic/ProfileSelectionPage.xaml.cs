namespace HealthCare.Views;

public partial class ProfileSelectionPage : ContentPage
{
    public ProfileSelectionPage()
    {
        InitializeComponent();
    }
    private async void ToProfile(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }
}