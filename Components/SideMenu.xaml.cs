namespace HealthCare.Components
{
    public partial class SideMenu : FlyoutPage
    {
        public SideMenu()
        {
            InitializeComponent();
        }

        // Evento para abrir el menú lateral
        private void OpenMenu(object sender, EventArgs e)
        {
            IsPresented = !IsPresented;
        }
    }
}

