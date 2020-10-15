using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PlatformTest.Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlatformTest.WebApi.Controllers
{
    [ApiController]
    public abstract class StorageControllerBase<T> : ControllerBase
    {
        private readonly IStorageService<T> _service;

        protected StorageControllerBase(IStorageService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Get([FromRoute]string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out var contentType))
            {
                contentType = "text/plain";
            }

            try
            {
                var result = await _service.GetFile(filename);
                return File(result, contentType);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{filename}")]
        public async Task<IActionResult> Delete([FromRoute]string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }

            try
            {
                await _service.Delete(filename);
                return NoContent();
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            using var stream = file.OpenReadStream();
            var buffer = new byte[file.Length];
            await stream.ReadAsync(buffer);
            try
            {
                await _service.Save(file.FileName, buffer);
                return CreatedAtAction(nameof(Get), new { file.FileName }, new { buffer });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
