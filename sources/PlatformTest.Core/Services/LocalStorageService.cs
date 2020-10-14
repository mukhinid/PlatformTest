using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
using System.IO;

namespace PlatformTest.Core.Services
{
    public sealed class LocalStorageService : IStorageService<LocalStorage>
    {
        private readonly string _rootDir;

        public LocalStorageService()
        {
            _rootDir = "./";
        }

        public void Save(string filename, byte[] data)
        {
            var path = Path.Combine(_rootDir, filename);
            File.WriteAllBytes(path, data);
        }
    }
}
