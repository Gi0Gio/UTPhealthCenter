
using HealthCare.ViewModel;

namespace HealthCare.Views.Students
{
    public partial class PatientRegisterPage : ContentPage
    {
        private readonly PatientRegisterViewModel _viewModel;

        public PatientRegisterPage(int userId)
        {
            InitializeComponent();
            _viewModel = new PatientRegisterViewModel(userId);
            BindingContext = new PatientRegisterViewModel(userId);
        }
    }
}
