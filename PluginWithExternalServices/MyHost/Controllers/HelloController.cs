using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Contract;
using Prise;

namespace MyHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        private readonly IHelloPlugin _helloPlugin;

        public HelloController(IHelloPlugin helloPlugin)
        {
            _helloPlugin = helloPlugin;
        }

        [HttpGet]
        public string Get([FromQuery]string input)
        {
            return _helloPlugin.SayHello(input);
        }

        [HttpGet("async")]
        public async Task<string> GetAsync([FromQuery]string input)
        {
            return  await _helloPlugin.SayHelloAsync(input);
        }

        [HttpGet("int")]
        public async Task<string> GetIntAsync([FromQuery]string input)
        {
            return "Not implemented";
            //return (await _helloPlugin.SayHelloIntAsync(0)).ToString();
        }
    }
}
