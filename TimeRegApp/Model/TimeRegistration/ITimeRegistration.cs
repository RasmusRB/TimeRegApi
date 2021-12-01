using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.TimeRegistration
{
    public interface ITimeRegistration
    {
        TimeRegistration CreateTimeRegistration(DynamicParameters tReg);

        TimeRegistration GetRegistrationByUser(int fkuser_id);
    }
}
