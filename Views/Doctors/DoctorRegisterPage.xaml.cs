namespace HealthCare.Views.Doctors;

public partial class DoctorRegisterPage : ContentPage
{
    private int _userId;
    public DoctorRegisterPage(int userId)
    {
        InitializeComponent();
        _userId = userId;
        BindingContext = new DoctorRegisterViewModel(_userId);
    }
    private async void ToLogin(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
}