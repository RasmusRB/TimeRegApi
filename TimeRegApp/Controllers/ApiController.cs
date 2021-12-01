using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TimeReg_Api.TimeRegApp.Model.Account;

namespace TimeReg_Api.TimeRegApp.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/")]

    public class ApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccount _account;

        public ApiController(IConfiguration config, IAccount account)
        {
            _config = config;
            _account = account;
        }
    }
}
