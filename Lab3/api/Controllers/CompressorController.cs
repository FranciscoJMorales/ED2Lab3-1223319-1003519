using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        [HttpPost]
        public IActionResult Create([FromForm] IFormFile file)
        {
            try
            {
                using var content = new MemoryStream();
                file.CopyToAsync(content);
                var text = Encoding.ASCII.GetString(content.ToArray());
                var deg = JsonSerializer.Deserialize<int>(text);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
