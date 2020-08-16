using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rixtrema.BLL.Handlers.Interfaces;

namespace Rixtrema.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task<string> Get([FromServices] IPercentileHandler handler, int operationType)
        {
            var completePercentile = await handler.CompletePercentile(operationType);
            return completePercentile;
        }
    }
}