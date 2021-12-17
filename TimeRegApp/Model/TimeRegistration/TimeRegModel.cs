namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public class TimeRegModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // Two new properties
        public int ActivityId { get; set; }
        public int UserId { get; set; }

    }
}
