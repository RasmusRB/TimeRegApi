using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public interface ITimeRegistration
    {
        TimeRegModel CreateTimeStamp(DynamicParameters tReg);
        List<TimeRegModel> GetAllTimeregistrations();
        List<TimeRegModel> GetTimeRegByDate(DateTime date);
    }
}
