using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TimeReg_Api.TimeRegApp.Model;
using TimeReg_Api.TimeRegApp.Model.Account;
using TimeReg_Api.TimeRegApp.Model.Authentication;

namespace TimeReg_Api.TimeRegApp.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("user/")]

    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccount _account;
        private readonly IGenerateJwt _generateJwt;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration config, IAccount account, IGenerateJwt generateJwt, ILogger<UserController> logger)
        {
            _config = config;
            _account = account;
            _generateJwt = generateJwt;
            _logger = logger;
        }

        [HttpGet("getuser/")]
        public async Task<JsonResult> GetUserInfo(string email)
        {
            var user = await Task.FromResult(_account.GetUserByEmail(email));
            return Success(user);
        }

        [HttpPost("create/")]
        public async Task<JsonResult> CreateUser([FromForm] CreateUser cUser)
        {
            try
            {
                var userParams = new DynamicParameters();
                userParams.Add("@email", cUser.Email);
                userParams.Add("@password", BCrypt.Net.BCrypt.HashPassword(cUser.Password));
                userParams.Add("@firstname", cUser.Firstname);
                userParams.Add("@lastname", cUser.Lastname);
                userParams.Add("@phone", cUser.Telephone);
                // TODO change role distribution
                userParams.Add("@role", "user");

                var user = await Task.FromResult(_account.CreateUser(userParams));

                // Simply returns the created user Id
                return Success(user.Id);

                // Logging errors to terminal
            }
            catch (Npgsql.PostgresException e)
            {
                _logger.LogWarning($"User with Email already exists - Exception: {e.ToString()}");

                return new JsonResult("User already exists!")
                {
                    StatusCode = 409
                };
            }
        }

        [HttpPost("login/")]
        public async Task<JsonResult> Login([FromForm] Login login)
        {
            try
            {
                var user = await Task.FromResult(_account.GetUserByEmail(login.Email));
                if (user is null)
                {
                    return InvalidRequest("Wrong Username or Password.");
                }

                // Check if the password the user entered is correct.
                bool verified =
                  BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

                // Never say which one of the two is wrong.
                if (!verified)
                {
                    return InvalidRequest("Wrong Username or Password.");
                }
                return Success(_generateJwt.GenerateJWT(user));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error logging in user with email {login.Email} - Exception {e.ToString()}");
                return InternalError();
            }
        }

        // TODO needs authorization
        [HttpDelete("delete")]
        public async Task<JsonResult> DeleteUser(string userEmail)
        {
            try
            {
                var result = await Task.FromResult(_account.DeleteUser(userEmail));
                if (!result)
                {
                    return InvalidRequest();
                }
                return Success(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error Deleting User - Exception: {e.ToString()}");
                return InternalError();
            }
        }

        private JsonResult InternalError()
        {
            return new JsonResult("Internal Server Error")
            {
                StatusCode = 500
            };
        }
        private JsonResult InvalidRequest()
        {
            return new JsonResult("Invalid Request")
            {
                StatusCode = 400
            };
        }
        private JsonResult InvalidRequest(object response)
        {
            return new JsonResult(response)
            {
                StatusCode = 400
            };
        }
        private JsonResult Success(object response)
        {
            return new JsonResult(response)
            {
                StatusCode = 200
            };
        }
    }
}
