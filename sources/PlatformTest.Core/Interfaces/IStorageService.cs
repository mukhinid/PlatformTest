using System.Collections.Generic;

namespace PlatformTest.Core.Interfaces
{
    public interface IStorageService<T>
    {
        IEnumerable<string> GetAll();
        byte[] GetFile(string name);
        void Save(string filename, byte[] data);
    }
}
