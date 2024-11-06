using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class DoctorRegisterViewModel : INotifyPropertyChanged
{
    private int _userId;
    public DoctorRegisterViewModel(int userId)
    {
        _userId = userId;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task RegisterDoctor()
    {
        var doctorDto = new
        {
            UserId = _userId,
            // Otros datos del doctor
        };

        var jsonContent = JsonSerializer.Serialize(doctorDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/doctors", content);

            if (response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Registro exitoso", "Doctor registrado exitosamente.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar al doctor.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}

