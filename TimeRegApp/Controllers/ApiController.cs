using Dapper;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using TimeRegApi.TimeRegApp.Model;
using Microsoft.AspNetCore.Authorization;
using TimeReg_Api.TimeRegApp.Model.Activity;
using TimeReg_Api.TimeRegApp.Model.Authentication;

namespace TimeReg_Api.TimeRegApp.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/")]

    public class ApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IActivity _activity;
        private readonly ILogger<ApiController> _logger;
        private readonly IGenerateJwt _generateJwt;

        public ApiController(IConfiguration config, IActivity activity, ILogger<ApiController> logger, IGenerateJwt generateJwt)
        {
            _config = config;
            _activity = activity;
            _logger = logger;
            _generateJwt = generateJwt;
        }

        [Authorize(Policy = "SessionToken")]
        [HttpPost("activitycreate/")]
        public async Task<JsonResult> CreateActivity([FromForm] CreateActivity cActivity)
        {
            try
            {
                var actParams = new DynamicParameters();
                actParams.Add("@activity", cActivity.Activity);

                var act = await Task.FromResult(_activity.CreateActivity(actParams));

                return Success(act);

            }
            catch (Npgsql.PostgresException e)
            {
                // Logging errors to terminal by using ILogger class
                _logger.LogWarning($"Exception: {e.ToString()}");
                return InternalError();
            }
        }

        [HttpGet("activityid/{id}")]
        public async Task<JsonResult> GetRegistrationById(int id)
        {
            try
            {
                var act = await Task.FromResult(_activity.GetRegistrationById(id));
                return Success(act);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Couldn't find registration with that id - Except {e.ToString()}");
                return InternalError();
            }
        }

        [HttpPost("activityupdate/{id}")]
        public async Task<JsonResult> UpdateRegistration(int id, [FromForm] string activity)
        {
            try
            {
                var actParams = new DynamicParameters();
                actParams.Add("@activity", activity);

                var act = await Task.FromResult(_activity.UpdateRegistrationById(id, activity));
                return Success(act);
            }
            catch (Exception e)
            {
                return InternalError();
            }
        }

        [Authorize(Policy = "SessionToken")]
        [HttpDelete("activitydelete/{id}")]
        public async Task<JsonResult> DeleteRegById(int id)
        {
            try
            {
                var act = await Task.FromResult(_activity.DeleteRegistrationById(id));

                return Success(act);
            }
            catch (Exception e)
            {
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
