namespace HealthCare.Views.Students.Appointments;

public partial class PatientPendingPage : ContentPage
{
    public PatientPendingPage()
    {
        InitializeComponent();
    }
    private async void Request(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PatientRequestPage());
    }
}