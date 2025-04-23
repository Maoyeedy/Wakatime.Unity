#if (UNITY_EDITOR)
using System;
using System.IO;
using UnityEngine;

namespace Wakatime
{
    public static class EnvReader
    {
        public static string GetWakatimeInPath()
        {
            var executableName = Application.platform == RuntimePlatform.WindowsEditor
                ? "wakatime-cli.exe"
                : "wakatime-cli";

            var pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathVariable))
            {
                return null;
            }

            var paths = pathVariable.Split(Path.PathSeparator);

            foreach (var path in paths)
            {
                try
                {
                    var fullPath = Path.Combine(path.Trim(), executableName);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
                catch (System.Security.SecurityException)
                {
                    Debug.LogWarning($"Permission denied accessing path: {path}");
                }
            }

            return null;
        }


        public class WakaConfig
        {
            public string ApiKey { get; set; }
            public string ApiUrl { get; set; }
        }

        // This parsing is written by gemini, and I kept its comments.
        public static WakaConfig GetConfigInEnv()
        {
            var home = "";
            try
            {
                home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                if (string.IsNullOrEmpty(home))
                {
                    Debug.LogWarning("Could not determine user home directory.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error getting user home directory: {ex.Message}");
                return null;
            }

            var cfgPath = Path.Combine(home, ".wakatime.cfg");

            if (!File.Exists(cfgPath))
            {
                Debug.LogWarning($"WakaTime config file '{cfgPath}' does not exist.");
                return null;
            }

            var cfg = new WakaConfig();

            try
            {
                var currentSection = ""; // Handle simple INI sections like [settings]
                var lines = File.ReadAllLines(cfgPath);

                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();

                    // Skip empty lines and comments
                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#") || trimmedLine.StartsWith(";"))
                        continue;

                    // Check for section headers
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine;
                        continue;
                    }

                    // Only parse keys within the [settings] section or if no section defined yet (for compatibility)
                    if (currentSection.Equals("[settings]", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(currentSection))
                    {
                        var parts = trimmedLine.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();

                            if (key.Equals("api_key", StringComparison.OrdinalIgnoreCase))
                            {
                                cfg.ApiKey = value;
                            }
                            else if (key.Equals("api_url", StringComparison.OrdinalIgnoreCase) || key.Equals("api_uri", StringComparison.OrdinalIgnoreCase)) // Check both url/uri
                            {
                                cfg.ApiUrl = value;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.LogWarning($"Error reading WakaTime config file '{cfgPath}': {ex.Message}");
                return null;
            }
            catch (System.Security.SecurityException ex)
            {
                Debug.LogWarning($"Permission denied reading WakaTime config file '{cfgPath}': {ex.Message}");
                return null;
            }

            if (string.IsNullOrEmpty(cfg.ApiKey) && string.IsNullOrEmpty(cfg.ApiUrl))
            {
                Debug.LogWarning($"WakaTime config file '{cfgPath}' exists but don't have values.");
                return null;
            }

            Debug.Log($"Successfully read from '{cfgPath}'.");
            return cfg;
        }
    }
}
#endif
