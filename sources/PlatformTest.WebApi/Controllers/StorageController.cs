using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlatformTest.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService<LocalStorage> _localService;
        private readonly IStorageService<FtpStorage> _ftpService;

        public StorageController(IStorageService<LocalStorage> localService, IStorageService<FtpStorage> ftpService)
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
                        try
                        {
                            var result = await _localService.GetFile(filename);
                            return File(result, "text/plain");
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
                            return NoContent();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                default:
                    return NotFound();
            }
        }
    }
}
