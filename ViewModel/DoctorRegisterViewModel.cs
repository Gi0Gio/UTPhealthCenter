using HealthCare.Views;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

public class DoctorRegisterViewModel : INotifyPropertyChanged
{
    private readonly int _userId;

    private string _firstName;
    private string _lastName;
    private string _officeHours;
    private string _specialty;
    private string _phoneNumber;

    public event PropertyChangedEventHandler PropertyChanged;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string OfficeHours
    {
        get => _officeHours;
        set
        {
            _officeHours = value;
            OnPropertyChanged(nameof(OfficeHours));
        }
    }

    public string Specialty
    {
        get => _specialty;
        set
        {
            _specialty = value;
            OnPropertyChanged(nameof(Specialty));
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }

    public ICommand RegisterCommand => new Command(async () => await RegisterDoctor());

    public DoctorRegisterViewModel(int userId)
    {
        _userId = userId;
    }

    protected virtual void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public async Task RegisterDoctor()
    {
        var doctorDto = new
        {
            UserId = _userId,
            FirstName = this.FirstName,
            LastName = this.LastName,
            OfficeHours = this.OfficeHours,
            Specialty = this.Specialty,
            PhoneNumber = this.PhoneNumber
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
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un problema al registrar al doctor: {errorMessage}", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }
}
