using Dapper;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
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
        // Add various interfaces for use in the controller
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

        // Creates a single user
        [HttpPost("create/")]
        public async Task<JsonResult> CreateUser([FromForm] CreateUser cUser)
        {
            try
            {
                var userParams = new DynamicParameters();
                userParams.Add("@email", cUser.Email);
                // Using BCrypt nuget package to hash the entered password and dispatch it to the db
                userParams.Add("@password", BCrypt.Net.BCrypt.HashPassword(cUser.Password));
                userParams.Add("@firstname", cUser.Firstname);
                userParams.Add("@lastname", cUser.Lastname);
                userParams.Add("@phone", cUser.Telephone);
                // TODO change role distribution
                userParams.Add("@role", "user");

                // Call method from manager class to actually create the user
                var user = await Task.FromResult(_account.CreateUser(userParams));

                // Simply returns the created user Id
                return Success(user.Id);

            }
            catch (Npgsql.PostgresException e)
            {
                // Logging errors to terminal by using ILogger class
                _logger.LogWarning($"User with Email already exists - Exception: {e.ToString()}");

                return new JsonResult("User already exists!")
                {
                    StatusCode = 409
                };
            }
        }

        // Posts login credentials
        [HttpPost("login/")]
        public async Task<JsonResult> Login([FromForm] Login login)
        {
            try
            {
                // Checks if there's a user in DB with entered email
                var user = await Task.FromResult(_account.GetUserByEmail(login.Email));
                if (user is null)
                {
                    return InvalidRequest("Wrong Username or Password.");
                }

                // Check if the password the user entered is correct.
                // Verify is a built in BCrypt function
                bool verified =
                  BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

                // Never say which one of the two is wrong.
                if (!verified)
                {
                    return InvalidRequest("Wrong Username or Password.");
                }
                return Success(_generateJwt.GenerateJWT("session_token", login.Email, user));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error logging in user with email {login.Email} - Exception {e.ToString()}");
                return InternalError();
            }
        }

        // Deletes a single user
        [HttpDelete("delete")]
        // TODO fix - Should work, but doesn't [Authorize(Roles = "RequireAdministratorRole")]
        public async Task<JsonResult> DeleteUser(string userEmail)
        {
            try
            {
                // Deletes a user based on the entered email
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
