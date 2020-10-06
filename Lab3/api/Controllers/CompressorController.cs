using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Compressors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompressorController : ControllerBase
    {
        readonly IWebHostEnvironment env;

        public CompressorController(IWebHostEnvironment _env)
        {
            env = _env;
        }

        [HttpPost]
        [Route("/api/compress/{name}")]
        public IActionResult Compress([FromForm] IFormFile file, string name)
        {
            try
            {
                using var content = new MemoryStream();
                file.CopyToAsync(content);
                content.Position = 0;   
                using var reader = new StreamReader(content);
                var text = reader.ReadToEnd();
                var compressor = new HuffmanCompressor(env.ContentRootPath);
                string path = compressor.Compress(text, file.FileName, name);
                var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                return File(fileStream, "text/plain");
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/api/decompress")]
        public IActionResult Decompress([FromForm] IFormFile file)
        {
            try
            {
                using var content = new MemoryStream();
                file.CopyToAsync(content);
                content.Position = 0;
                using var reader = new StreamReader(content);
                var text = reader.ReadToEnd();
                var compressor = new HuffmanCompressor(env.ContentRootPath);
                string path = compressor.Decompress(text);
                var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                return File(fileStream, "text/plain");
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
