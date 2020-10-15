using Microsoft.Extensions.Options;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Data.Descriminators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PlatformTest.Data.Services
{
    public sealed class FtpStorageService : IStorageService<Ftp>
    {
        private readonly FtpStorageOptions _options;

        public FtpStorageService(IOptions<FtpStorageOptions> options)
        {
            _options = options.Value;
        }

        public Task Delete(string filename)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            var request = (FtpWebRequest)WebRequest.Create(_options.Url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_options.Login, _options.Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            using var response = (FtpWebResponse)(await request.GetResponseAsync());
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            var result = new List<string>();

            string line = await reader.ReadLineAsync();
            while (!string.IsNullOrEmpty(line))
            {
                result.Add(line);
                line = await reader.ReadLineAsync();
            }

            return result;
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
