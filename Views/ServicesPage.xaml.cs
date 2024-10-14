namespace HealthCare.Views;

public partial class ServicesPage : ContentPage
{
    public ServicesPage()
    {
        InitializeComponent();
    }

    //Manejo del evento tapped 
    private async void Login(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void Recetas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MedicinesPage());   
    }
    private async void Citas(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CitasPage());
    }

    private async void Profile(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfileSelectionPage());
    }

    private async void StudentRegister(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StudentRegisterPage());
    }
}