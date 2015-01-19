namespace WinAppDriver
{
    using Newtonsoft.Json;

    internal class Capabilities
    {
        [JsonProperty("platformName")]
        public string PlatformName { get; set; }

        public string PackageName { get; set; }

        public string App { get; set; }

        public string MD5 { get; set; }
    }
}