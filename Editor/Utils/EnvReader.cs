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
            string executableName = Application.platform == RuntimePlatform.WindowsEditor
                ? "wakatime-cli.exe"
                : "wakatime-cli";

            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathVariable))
            {
                return null;
            }

            var paths = pathVariable.Split(Path.PathSeparator);

            foreach (var path in paths)
            {
                try
                {
                    string fullPath = Path.Combine(path.Trim(), executableName);
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
    }
}


#endif
