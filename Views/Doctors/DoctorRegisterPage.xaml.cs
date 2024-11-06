namespace HealthCare.Views.Doctors;

public partial class DoctorRegisterPage : ContentPage
{
    public DoctorRegisterPage(int userId)
    {
        InitializeComponent();
        BindingContext = new DoctorRegisterViewModel(userId);
    }
}
