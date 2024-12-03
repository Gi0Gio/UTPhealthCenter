namespace HealthCare.Views.Students.Appointments;
using HealthCare.ViewModel;

public partial class PatientPendingPage : ContentPage
{
    public PatientPendingPage()
    {
        InitializeComponent();
        BindingContext = new PatientRequestPage();
    }
    private async void Request(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PatientRequestPage());
    }
}