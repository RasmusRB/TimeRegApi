using TimeReg_Api.TimeRegApp.Model.Account;

namespace TimeReg_Api.TimeRegApp.Model.Authentication
{
    public interface IGenerateJwt
    {
        string GenerateJWT(string type, string subject, User user);
        string ValidateToken(string token, string expectedType);
    }
}
