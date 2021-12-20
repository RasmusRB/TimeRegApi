using Dapper;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using TimeRegApi.TimeRegApp.Model;
using Microsoft.AspNetCore.Authorization;
using TimeReg_Api.TimeRegApp.Model.Activity;
using TimeReg_Api.TimeRegApp.Model.Authentication;
using TimeReg_Api.TimeRegApp.Model;
using TimeReg_Api.TimeRegApp.Model.TimeRegistration;

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
        private readonly ITimeRegistration _timeRegistration;

        public ApiController(IConfiguration config, IActivity activity, ILogger<ApiController> logger, IGenerateJwt generateJwt, ITimeRegistration timeRegistration)
        {
            _config = config;
            _activity = activity;
            _logger = logger;
            _generateJwt = generateJwt;
            _timeRegistration = timeRegistration;
        }

        // Now with ActivityID and UserId
        [HttpPost("timeregcreate/")]
        public async Task<JsonResult> CreateTimeRegistration([FromForm] CreateTimeRegistration tReg)
        {
            try
            {
                var timeParams = new DynamicParameters();
                timeParams.Add("@timereg_start", tReg.Started);
                timeParams.Add("@timereg_end", tReg.Ended);
                timeParams.Add("@activity_id", tReg.ActivityId);

                // Somehow get user_id from session.
                timeParams.Add("@user_id", tReg.UserId);

                var asd = await Task.FromResult(_timeRegistration.CreateTimeStamp(timeParams));

                return Success(asd);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Something went bad! - Exception: {e.ToString()}");
                return InternalError();
            }
        }

        [Authorize(Policy = "SessionToken")]
        [HttpGet("activities/")]
        public async Task<JsonResult> GetallActivities()
        {
            try
            {
                var act = await Task.FromResult(_activity.GetAllActivities());

                return Success(act);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Something went bad! - Exception: {e.ToString()}");
                return InternalError();
            }
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
