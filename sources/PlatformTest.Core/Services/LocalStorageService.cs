using Microsoft.Extensions.Options;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Core.Storages;
using System.IO;

namespace PlatformTest.Core.Services
{
    public sealed class LocalStorageService : IStorageService<LocalStorage>
    {
        private readonly LocalStorageOptions _options;

        public LocalStorageService(IOptions<LocalStorageOptions> options)
        {
            _options = options.Value;
        }

        public void Save(string filename, byte[] data)
        {
            var path = Path.Combine(_options.RootDir, filename);
            File.WriteAllBytes(path, data);
        }
    }
}
