using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformTest.Core.Services
{
    public sealed class FtpStorageService : IStorageService<FtpStorage>
    {
        public Task<IEnumerable<string>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetFile(string name)
        {
            throw new NotImplementedException();
        }

        public Task Save(string filename, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
