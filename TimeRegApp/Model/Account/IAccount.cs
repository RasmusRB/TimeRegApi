using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.Account
{
    public interface IAccount
    {
        User CreateUser(DynamicParameters userParams);
        User GetUserByEmail(string email);
        bool DeleteUser(string userEmail);
    }
}
