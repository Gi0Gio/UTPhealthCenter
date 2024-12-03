namespace HealthCare.Views.Doctors;
using HealthCare.Views.Doctors.Citas;

public partial class DoctorHomePage : ContentPage
{
    public DoctorHomePage()
    {
        InitializeComponent();
    }

    //Manejo del evento tapped 
    private async void Citas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TusCitas());
    }

    private async void Prescripcion(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Prescripcion());
    }

    private async void Pacientes(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Patients());
    }

    private async void Referencias(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Referencia());
    }

    private async void Historial(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MedicalProcedure());
    }

}