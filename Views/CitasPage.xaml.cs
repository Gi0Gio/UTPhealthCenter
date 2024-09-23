using HealthCare.ViewModel;

namespace HealthCare.Views
{
    public partial class CitasPage : ContentPage
    {
        private PatientAppointmentViewModel _viewModel;

        public CitasPage()
        {
            InitializeComponent();
            _viewModel = new PatientAppointmentViewModel();
            BindingContext = _viewModel;
        }

        private void ShowPendingCitas(object sender, EventArgs e)
        {
            PendingCitasSection.IsVisible = true;
            ScheduledCitasSection.IsVisible = false;
            AppointmentFormSection.IsVisible = false;
        }

        private void ShowScheduledCitas(object sender, EventArgs e)
        {
            PendingCitasSection.IsVisible = false;
            ScheduledCitasSection.IsVisible = true;
            AppointmentFormSection.IsVisible = false;
        }

        private void ShowAppointmentForm(object sender, EventArgs e)
        {
            PendingCitasSection.IsVisible = false;
            ScheduledCitasSection.IsVisible = false;
            AppointmentFormSection.IsVisible = true;
        }

        private async void OnSaveAppointment(object sender, EventArgs e)
        {
            await _viewModel.SaveAppointment();
        }

        private async void OnDeleteAppointment(object sender, EventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.BindingContext as AppointmentDto;

            if (appointment != null)
            {
                await _viewModel.DeleteAppointment(appointment);
            }
        }
    }
}
