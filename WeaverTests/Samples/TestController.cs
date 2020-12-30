namespace WebHarness
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController, Route("Sample")]
    public class TestController : ControllerBase
    {
        public string SomeString
        {
            get;
            set;
        }
            
        = "SomeString";
        [HttpGet]
        public string SomeMethod()
        {
            return $"Hello {SomeString}";
        }
    }
}