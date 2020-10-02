using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompressorController : ControllerBase
    {
        IWebHostEnvironment env;

        public CompressorController(IWebHostEnvironment _env)
        {
            env = _env;
        }
    }
}
