namespace HealthCare.Views.Students.Appointments;

public partial class PatientRequestPage : ContentPage
{
	public PatientRequestPage()
	{
		InitializeComponent();
	}
	private async void ToPending(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new PatientPendingPage());
	}
}