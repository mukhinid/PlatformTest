using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PlatformTest.Core.Interfaces;
using PlatformTest.Data.Descriminators;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlatformTest.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService<Local> _localService;
        private readonly IStorageService<Ftp> _ftpService;

        public StorageController(IStorageService<Local> localService, IStorageService<Ftp> ftpService)
        {
            _localService = localService;
            _ftpService = ftpService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] string storage)
        {
            if (string.IsNullOrEmpty(storage))
            {
                return BadRequest();
            }

            switch(storage)
            {
                case "local":
                    try
                    {
                        var result = await _localService.GetAll();
                        return Ok(result);
                    }
                    catch(Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                case "ftp":
                    try
                    {
                        var result = await _ftpService.GetAll();
                        return Ok(result);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                default:
                    return BadRequest();
            }
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> Get([FromQuery]string storage, [FromRoute]string filename)
        {
            if (string.IsNullOrEmpty(storage))
            {
                return BadRequest();
            }

            switch(storage)
            {
                case "local":
                    if (string.IsNullOrEmpty(filename))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        var provider = new FileExtensionContentTypeProvider();
                        if (!provider.TryGetContentType(filename, out var contentType))
                        {
                            contentType = "text/plain";
                        }

                        try
                        {
                            var result = await _localService.GetFile(filename);
                            return File(result, contentType);
                        }
                        catch(FileNotFoundException ex)
                        {
                            return NotFound(ex.Message);
                        }
                        catch(Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                default:
                    return BadRequest();
            }
        }

        [HttpDelete("{filename}")]
        public async Task<IActionResult> Delete([FromQuery]string storage, [FromRoute]string filename)
        {
            if (string.IsNullOrEmpty(storage))
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }

            switch (storage)
            {
                case "local":
                    try
                    {
                        await _localService.Delete(filename);
                        return NoContent();
                    }
                    catch(FileNotFoundException ex)
                    {
                        return NotFound(ex.Message);
                    }
                    catch(Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                default:
                    return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery]string storage, [FromForm]IFormFile file)
        {
            if (string.IsNullOrEmpty(storage))
            {
                return BadRequest();
            }
            if (file == null)
            {
                return BadRequest();
            }

            switch(storage)
            {
                case "local":
                    using (var stream = file.OpenReadStream())
                    {
                        var buffer = new byte[file.Length];
                        await stream.ReadAsync(buffer);
                        try
                        {
                            await _localService.Save(file.FileName, buffer);
                            return CreatedAtAction(nameof(Get), new { file.FileName }, new { buffer });
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                default:
                    return BadRequest();
            }
        }
    }
}
