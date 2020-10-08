using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        [HttpGet]
        public IEnumerable<Compression> Get()
        {
            return GetCompressions();
        }

        [HttpPost]
        [Route("/api/compress/{name}")]
        public IActionResult Compress([FromForm] IFormFile file, string name)
        {
            try
            {
                //evita que se manden archivos vacios
                if (file == null)
                    return StatusCode(500);


                string path = env.ContentRootPath + "\\" + file.FileName;
                using var saver = new FileStream(path, FileMode.Create);
                file.CopyToAsync(saver);
                saver.Close();
                using var fileWritten = new FileStream(path, FileMode.OpenOrCreate);
                using var reader = new BinaryReader(fileWritten);
                byte[] buffer = new byte[0];
                while (fileWritten.Position < fileWritten.Length)
                {
                    int index = buffer.Length;
                    Array.Resize<byte>(ref buffer, index + 100000);
                    byte[] aux = reader.ReadBytes(100000);
                    aux.CopyTo(buffer, index);
                }
                reader.Close();
                fileWritten.Close();
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 0)
                    {
                        Array.Resize<byte>(ref buffer, i);
                        break;
                    }
                }
                if(buffer.Length > 0)
                {
                    var compressor = new HuffmanCompressor(env.ContentRootPath);
                    path = compressor.Compress(buffer, file.FileName, name);
                    var fileStream = new FileStream(path, FileMode.OpenOrCreate);
                    return File(fileStream, "text/plain");
                }
                return StatusCode(500);
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

        [HttpGet]
        [Route("/api/compressions")]
        public IEnumerable<Compression> GetCompressions()
        {
            var huffman = new HuffmanCompressor(env.ContentRootPath);
            var list = huffman.GetCompressions();
            
                return list;
            
        }
    }
}
