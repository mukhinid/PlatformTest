using Microsoft.Extensions.Options;
using PlatformTest.Core.Interfaces;
using PlatformTest.Core.Options;
using PlatformTest.Data.Descriminators;
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

        public async Task Delete(string filename)
        {
            var url = Path.Combine(_options.Url, filename);
            await MakeRequest(WebRequestMethods.Ftp.DeleteFile, url);
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            var result = await MakeRequest(WebRequestMethods.Ftp.ListDirectory, _options.Url);
            var directories = new List<string>();

            using var ms = new MemoryStream(result);
            using var reader = new StreamReader(ms);
            string line = await reader.ReadLineAsync();
            while (!string.IsNullOrEmpty(line))
            {
                directories.Add(line);
                line = await reader.ReadLineAsync();
            }

            return directories;
        }

        public async Task<byte[]> GetFile(string name)
        {
            var url = Path.Combine(_options.Url, name);
            return await MakeRequest(WebRequestMethods.Ftp.DownloadFile, url);
        }

        public async Task Save(string filename, byte[] data)
        {
            var url = Path.Combine(_options.Url, filename);
            await MakeRequest(WebRequestMethods.Ftp.UploadFile, url, data);
        }

        private async Task<byte[]> MakeRequest(string method, string uri, byte[] requestBody = null)
        {
            var request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Credentials = new NetworkCredential(_options.Login, _options.Password);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            if (requestBody != null)
            {
                using var requestMemStream = new MemoryStream(requestBody);
                using var requestStream = await request.GetRequestStreamAsync();
                requestMemStream.CopyTo(requestStream);
            }

            using var response = (FtpWebResponse)(await request.GetResponseAsync());
            using var responseBody = new MemoryStream();
            response.GetResponseStream().CopyTo(responseBody);
            return responseBody.ToArray();
        }
    }
}
