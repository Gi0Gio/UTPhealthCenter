namespace HealthCare.Models
{
    public class Appointment
    {
        public int id { get; set; }
        public int patientId { get; set; }
        public int doctorId { get; set; }   

        public DateTime appointmentDate { get; set; }
        public string description { get; set; }
        public string status { get; set; }      
        public string type { get; set; }    
        public int clinicId  { get; set; }  

        public object patient { get; set; }
        public object doctor { get; set; }  
        public object clinic { get; set; }  
    }
}
