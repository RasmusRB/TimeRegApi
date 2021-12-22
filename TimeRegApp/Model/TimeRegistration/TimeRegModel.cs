namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public class TimeRegModel
    {
        public int timereg_id { get; set; }
        public DateTime timereg_created { get; set; }
        public DateTime timereg_start { get; set; }
        public DateTime timereg_end { get; set; }
        public string timereg_comment { get; set; }
        public int activity_id { get; set; }
        public int user_id { get; set; }
    }
}
