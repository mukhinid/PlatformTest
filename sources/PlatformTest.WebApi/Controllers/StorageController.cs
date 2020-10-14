using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromQuery]string storage, [FromForm]IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(storage))
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
                        _localService.Save(file.FileName, buffer);

                        return NoContent();
                    }
                default:
                    return NotFound();
            }
        }
    }
}
