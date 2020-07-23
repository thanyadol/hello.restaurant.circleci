using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace hello.restaurant.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {

        [EnableCors("AllowCors")]
        [Route("")]
        [HttpGet]
        public ActionResult<string> Index()
        {
            return "Hello World with .NET Framework CORE 2.2 Web API at " + DateTime.Now;
        }

        
    }
}