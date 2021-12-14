using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.Activity
{
    public interface IActivity
    {
        Activity CreateActivity(DynamicParameters aCreate);
        Activity GetRegistrationById(int id);
        Activity UpdateRegistrationById(int id, string activity);
        Activity DeleteRegistrationById(int id);
    }
}