//public class UserModel
//{
//    public int Id { get; set; }
//    public string Username { get; set; }
//    public string PasswordHash { get; set; }
//    public string Email { get; set; }
//    public int RoleId { get; set; }
//    public DateTime RegistrationDate { get; set; }
//    public bool IsActive { get; set; }
//    public object Role { get; set; } // Esto puede ser null según la respuesta del API
//}
namespace HealthCare.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}