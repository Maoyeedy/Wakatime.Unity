#if (UNITY_EDITOR)

using System.Collections.ObjectModel;
using System.Linq;

namespace Wakatime
{
    public class CliParameters
    {
        public string Key { get; set; }
        public string File { get; set; }
        public string Time { get; set; }
        public string Plugin { get; set; }
        public string ApiUri { get; set; } // Add ApiUri property
        public HeartbeatCategories? Category { get; set; }
        public EntityTypes? EntityType { get; set; }
        public bool IsWrite { get; set; }
        public string Project { get; set; }
        public bool HasExtraHeartbeats { get; set; }

        public string[] ToArray(bool obfuscate = false)
        {
            var parameters = new Collection<string>
            {
                "--key",
                obfuscate ? $"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXX{Key.Substring(Key.Length - 4)}" : Key,
                "--entity",
                File,
                "--time",
                Time,
                "--plugin",
                Plugin,
                "--language",
                "UnityEditor" // Otherwise it will be "Unknown"
            };

            // Add ApiUri if it's not the default
            if (!string.IsNullOrEmpty(ApiUri) && ApiUri != "https://api.wakatime.com/api/v1/")
            {
                parameters.Add("--api-url"); // Correct parameter name is --api-url
                parameters.Add(ApiUri);
            }

            if (Category != null)
            {
                parameters.Add("--category");
                parameters.Add(Category.GetDescription());
            }

            if (EntityType != null)
            {
                parameters.Add("--entity-type");
                parameters.Add(EntityType.GetDescription());
            }

            // ReSharper disable once InvertIf
            if (!string.IsNullOrEmpty(Project))
            {
                parameters.Add("--project");
                parameters.Add(Project);
            }

            if (IsWrite)
                parameters.Add("--write");

            if (HasExtraHeartbeats)
                parameters.Add("--extra-heartbeats");

            return parameters.ToArray();
        }
    }
}
#endif
