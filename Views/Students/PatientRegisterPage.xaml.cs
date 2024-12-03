using HealthCare.ViewModel;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace HealthCare.Views.Students
{
    public partial class PatientRegisterPage : ContentPage
    {
        private readonly PatientRegisterViewModel _viewModel;

        public PatientRegisterPage(int userId)
        {
            InitializeComponent();
            _viewModel = new PatientRegisterViewModel(userId);
        }
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var dni = DniEntry.Text;
            var address = AddressEntry.Text;
            var birthDate = BirthDatePicker.Date;
            var phoneNumber = PhoneNumberEntry.Text;

            // Descompone la tupla en isRegistered y errorMessage
            var (isRegistered, errorMessage) = await _viewModel.RegisterPatient(firstName, lastName, dni, address, birthDate, phoneNumber);

            if (isRegistered)
            {
                await DisplayAlert("Registro exitoso", "El paciente ha sido registrado exitosamente.", "OK");
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                // Usa el mensaje de error devuelto por el servidor
                await DisplayAlert("Error", $"Hubo un problema al registrar al paciente: {errorMessage}", "OK");
            }
        }

    }
}
