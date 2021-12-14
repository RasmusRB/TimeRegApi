using Dapper;

namespace TimeReg_Api.TimeRegApp.Model.Account
{
    // Interface for manager class (account)
    public interface IAccount
    {
        User CreateUser(DynamicParameters userParams);
        User UpdateUser(int id, DynamicParameters userParams);
        User GetUserByEmail(string email);
        List<User> GetAllUsers();
        bool DeleteUser(string userEmail);
    }
}
