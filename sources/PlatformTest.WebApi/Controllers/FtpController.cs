using Microsoft.AspNetCore.Mvc;
using PlatformTest.Core.Interfaces;
using PlatformTest.Data.Descriminators;

namespace PlatformTest.WebApi.Controllers
{
    [Route("storage/[controller]")]
    public sealed class FtpController : StorageControllerBase<Ftp>
    {
        public FtpController(IStorageService<Ftp> service) : base(service)
        {
        }
    }
}
