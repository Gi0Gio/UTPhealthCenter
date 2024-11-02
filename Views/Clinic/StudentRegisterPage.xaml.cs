using HealthCare.ViewModel;

namespace HealthCare.Views;

public partial class StudentRegisterPage : ContentPage
{
    private StudentRegisterViewModel _viewModel;

    public StudentRegisterPage()
    {
        InitializeComponent();
        _viewModel = new StudentRegisterViewModel();
        BindingContext = _viewModel;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await _viewModel.RegisterPatient();
    }
}
