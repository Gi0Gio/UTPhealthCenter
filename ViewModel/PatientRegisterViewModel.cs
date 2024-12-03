using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthCare.ViewModel
{
    public class PatientRegisterViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly int _userId;

        public PatientRegisterViewModel(int userId)
        {
            _userId = userId;
            _httpClient = new HttpClient { BaseAddress = new Uri("https://giohealthcareservice-e0hba0b3f2d0bsh6.canadacentral-01.azurewebsites.net/api/") };
        }

        public async Task<(bool, string)> RegisterPatient(string firstName, string lastName, string dni, string address, DateTime birthDate, string phoneNumber)
        {
            var patientDto = new
            {
                UserId = _userId,
                FirstName = firstName,
                LastName = lastName,
                Dni = dni,
                Address = address,
                BirthDate = birthDate, // Envía el DateTime directamente
                PhoneNumber = phoneNumber
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonContent = JsonSerializer.Serialize(patientDto, options);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("Patients", content);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, errorMessage);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
