using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.Activity
{
    public interface IActivity
    {
        ActivityModel CreateActivity(DynamicParameters aCreate);
        ActivityModel GetRegistrationById(int id);
        ActivityModel UpdateRegistrationById(int id, string activity);
        ActivityModel DeleteRegistrationById(int id);
        IList<ActivityModel> GetAllActivities();
    }
}