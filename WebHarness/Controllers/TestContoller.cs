namespace WebHarness
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("Sample")]
    public class KiranController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("Hello World!!");
        }
    }
}