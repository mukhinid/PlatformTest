namespace PlatformTest.Core.Options
{
    public sealed class FtpStorageOptions
    {
        public const string Section = "FtpStorage";

        public string Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
