using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Storages;
using System;

namespace PlatformTest.Core.Services
{
    public sealed class FtpStorageService : IStorageService<FtpStorage>
    {
        public void Save(string filename, byte[] data)
        {
            throw new NotImplementedException("FtpStorage");
        }
    }
}
