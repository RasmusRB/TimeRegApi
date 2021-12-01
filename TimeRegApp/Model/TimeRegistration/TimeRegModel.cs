namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public class TimeRegModel
    {
        public int TimeReg_Id { get; set; }
        public DateTime TimeReg_Created { get; set; }
        public DateTime TimeReg_Start { get; set; }
        public DateTime TimeReg_End { get; set; }
        public int Public_Activity { get; set; }
        public int FkUser_Id { get; set; }
    }
}
