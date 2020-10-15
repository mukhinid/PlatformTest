using PlatformTest.Core.Interfaces;
using PlatformTest.Data.Descriminators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformTest.Data.Services
{
    public sealed class FtpStorageService : IStorageService<Ftp>
    {
        public Task Delete(string filename)
        {
            throw new NotImplementedException();
        }

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
