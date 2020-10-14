using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
using System;
using System.Collections.Generic;

namespace PlatformTest.Core.Services
{
    public sealed class FtpStorageService : IStorageService<FtpStorage>
    {
        public IEnumerable<string> GetAll()
        {
            throw new NotImplementedException();
        }

        public byte[] GetFile(string name)
        {
            throw new NotImplementedException();
        }

        public void Save(string filename, byte[] data)
        {
            throw new NotImplementedException("FtpStorage");
        }
    }
}
