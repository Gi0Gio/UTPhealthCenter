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

        private async void OnSaveAppointment(object sender, EventArgs e)
        {
            if (_viewModel.NewAppointment.id > 0)
                await _viewModel.UpdateAppointment();
            else
                await _viewModel.SaveAppointment();
        }

        private void OnEditAppointment(object sender, EventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.CommandParameter as AppointmentDto;

            if (appointment != null)
            {
                _viewModel.NewAppointment = new AppointmentDto
                {
                    id = appointment.id,
                    patientId = appointment.patientId,
                    doctorId = appointment.doctorId,
                    appointmentDate = appointment.appointmentDate,
                    description = appointment.description,
                    type = appointment.type,
                    status = appointment.status
                };

                _viewModel.SelectedPatient = _viewModel.Patients.FirstOrDefault(p => p.id == appointment.patientId);
                _viewModel.SelectedDoctor = _viewModel.Doctors.FirstOrDefault(d => d.id == appointment.doctorId);
            }
        }

        private async void OnDeleteAppointment(object sender, EventArgs e)
        {
            var button = sender as Button;
            var appointment = button?.CommandParameter as AppointmentDto;

            if (appointment != null)
            {
                bool isConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta cita?", "Sí", "No");
                if (isConfirmed)
                    await _viewModel.DeleteAppointment(appointment);
            }
        }
    }
}
