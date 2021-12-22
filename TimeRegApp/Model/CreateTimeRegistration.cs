namespace TimeReg_Api.TimeRegApp.Model
{
    public class CreateTimeRegistration
    {
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public string Comment { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }
    }
}