using Microsoft.Extensions.Options;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Core.Storages;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlatformTest.Core.Services
{
    public sealed class LocalStorageService : IStorageService<LocalStorage>
    {
        private readonly LocalStorageOptions _options;

        public LocalStorageService(IOptions<LocalStorageOptions> options)
        {
            _options = options.Value;

            if (!Directory.Exists(_options.RootDir))
            {
                Directory.CreateDirectory(_options.RootDir);
            }
        }

        public Task Delete(string filename)
        {
            if (!Directory.Exists(_options.RootDir))
            {
                throw new DirectoryNotFoundException();
            }

            var path = Path.Combine(_options.RootDir, filename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            File.Delete(path);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetAll()
        {
            if (!Directory.Exists(_options.RootDir))
            {
                throw new DirectoryNotFoundException();
            }

            return Task.FromResult(Directory.EnumerateFiles(_options.RootDir));
        }

        public async Task<byte[]> GetFile(string name)
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

            using var stream = File.OpenRead(path);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            return buffer;
        }

        public async Task Save(string filename, byte[] data)
        {
            if (!Directory.Exists(_options.RootDir))
            {
                throw new DirectoryNotFoundException();
            }

            var path = Path.Combine(_options.RootDir, filename);
            await File.WriteAllBytesAsync(path, data);
        }
    }
}
