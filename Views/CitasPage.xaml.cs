namespace HealthCare.Views
{
    public partial class CitasPage : ContentPage
    {
        public CitasPage()
        {
            InitializeComponent();
        }

        // Mostrar la sección de citas pendientes y ocultar las citas programadas
        private void ShowPendingCitas(object sender, EventArgs e)
        {
            PendingCitasSection.IsVisible = true;
            ScheduledCitasSection.IsVisible = false;
        }

        // Mostrar la sección de citas programadas y ocultar las citas pendientes
        private void ShowScheduledCitas(object sender, EventArgs e)
        {
            PendingCitasSection.IsVisible = false;
            ScheduledCitasSection.IsVisible = true;
        }
    }
}
