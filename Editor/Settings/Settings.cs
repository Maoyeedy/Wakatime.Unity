#if (UNITY_EDITOR)
using System;
using UnityEngine;
using System.IO;
using NUnit; // Added for Path.Combine

namespace Wakatime
{
    public class Settings : IDisposable, ICloneable
    {
        [Setting("WakaTime/Enabled")]
        public bool Enabled { get; set; } = false;

        [Setting("WakaTime/APIKey")]
        public string ApiKey { get; set; } = "";

        [Setting("WakaTime/ApiUri")]
        public string ApiUri { get; set; } = "https://api.wakatime.com/api/v1/";

        [Setting("WakaTime/GitOptions")]
        public GitClientTypes GitOptions { get; set; } = GitClientTypes.Disabled;

        [Setting("WakaTime/WakatimeHandlerType")]
        public WakatimeHandlerTypes WakatimeHandlerType { get; set; } = WakatimeHandlerTypes.WakatimeCli;

        [Setting("WakaTime/WakatimeCliBinary")]
        public string WakatimeCliBinary { get; set; } = "wakatime.exe";

        [Setting("WakaTime/LogLevel")]
        public LogLevels LogLevel { get; set; } = LogLevels.Informational;


        public string ProjectName => Application.productName;
        public TimeSpan HeartbeatFrequency => TimeSpan.FromMinutes(2);


        //[Setting("WakaTime/AutoUpdateCli")]
        //public bool AutoUpdateCli { get; set; } = true;
        //public string S3UrlPrefix => "https://wakatime-cli.s3-us-west-2.amazonaws.com/";
        //public string GithubDownloadPrefix => "https://github.com/wakatime/wakatime-cli/releases/download";
        //public string GithubReleasesAlphaUrl => "https://api.github.com/repos/wakatime/wakatime-cli/releases?per_page=1";
        //public string GithubReleasesStableUrl =>"https://api.github.com/repos/wakatime/wakatime-cli/releases/latest";


        public void RestoreDefaults()
        {
            // Enabled = false;
            // LogLevel = LogLevels.Informational;
            ApiKey = "";
            ApiUri = "https://api.wakatime.com/api/v1/";
            GitOptions = GitClientTypes.Disabled;
            WakatimeHandlerType = WakatimeHandlerTypes.WakatimeCli;

            // First try: use PATH
            string path = EnvReader.GetWakatimeInPath();
            if (!string.IsNullOrEmpty(path))
            {
                WakatimeCliBinary = path;
                Debug.Log($"Found wakatime-cli in PATH: {path}");
                return;
            }

            // Second try: Use Bundled
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                WakatimeCliBinary = Path.Combine(Application.dataPath, "Wakatime.Unity", "Editor", "Utils", "wakatime-cli.exe");
            }
            else
            {
                // Bundling macOS/Linux CLI could be overkill I think.
                Debug.LogWarning($"Please manually select WakaTime CLI executable.");
                WakatimeCliBinary = "";
                return;
            }
        }

        public void TryUseEnvConfig()
        {
            var config = EnvReader.GetConfigInEnv();
            if (config == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(config.ApiKey))
            {
                ApiKey = config.ApiKey;
            }
            if (!string.IsNullOrEmpty(config.ApiUrl))
            {
                ApiUri = config.ApiUrl;
            }
        }

        public void Dispose()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


#endif
