using Microsoft.Extensions.Options;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Core.Storages;
using System.Collections.Generic;
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

        public IEnumerable<string> GetAll()
        {
            if (Directory.Exists(_options.RootDir))
            {
                return Directory.EnumerateFiles(_options.RootDir);
            }

            throw new DirectoryNotFoundException();
        }

        public byte[] GetFile(string name)
        {
            if (!Directory.Exists(_options.RootDir))
            {
                throw new DirectoryNotFoundException();
            }
            
            var path = Path.Combine(_options.RootDir, name);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (var stream = File.OpenRead(path))
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer);
                return buffer;
            }
        }

        public void Save(string filename, byte[] data)
        {
            var path = Path.Combine(_options.RootDir, filename);
            File.WriteAllBytes(path, data);
        }
    }
}
