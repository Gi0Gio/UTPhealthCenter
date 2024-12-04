namespace HealthCare.Models;

public class PatientDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Dni { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
