namespace PlatformTest.Core.Interfaces
{
    public interface IStorageService<T>
    {
        void Save(string filename, byte[] data);
    }
}
