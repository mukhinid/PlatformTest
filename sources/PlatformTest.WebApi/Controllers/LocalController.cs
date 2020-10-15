using Microsoft.AspNetCore.Mvc;
using PlatformTest.Core.Interfaces;
using PlatformTest.Data.Descriminators;

namespace PlatformTest.WebApi.Controllers
{
    [Route("storage/[controller]")]
    public sealed class LocalController : StorageControllerBase<Local>
    {
        public LocalController(IStorageService<Local> service) : base(service)
        {
        }
    }
}
