namespace HealthCare.Views.Students;
using HealthCare.Views.Students.Appointments;
using HealthCare.Views.Students.Prescription;

public partial class StudentHomePage : ContentPage
{
    public StudentHomePage()
    {
        InitializeComponent();
    }
    private async void Request(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PatientRequestPage());
    }
    private async void History(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PatientHistoryPage());   
    }
    private async void Prescription(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PatientPrescriptionPage());
    }
}
