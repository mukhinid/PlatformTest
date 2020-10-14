using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformTest.Core.Interfaces
{
    public interface IStorageService<T>
    {
        Task<IEnumerable<string>> GetAll();
        Task<byte[]> GetFile(string name);
        Task Save(string filename, byte[] data);
    }
}
