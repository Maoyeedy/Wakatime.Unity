﻿#if (UNITY_EDITOR)
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wakatime
{
    public class SettingsWindow : EditorWindow
    {
        static Settings settings;
        [MenuItem("Tools/WakaTime")]
        static void Init()
        {
            SettingsWindow window = (SettingsWindow)GetWindow(typeof(SettingsWindow), false, "Wakatime settings");
            if (Plugin.Settings == null)
                Plugin.Initialize();
            settings = (Settings)Plugin.Settings.Clone();
            window.Show();
        }


        void OnGUI()
        {
            if (settings != null)
            {
                settings.Enabled = EditorGUILayout.Toggle("Enable WakaTime", settings.Enabled);
                settings.LogLevel = (LogLevels)EditorGUILayout.EnumPopup("Log level", settings.LogLevel);
                settings.GitOptions = (GitClientTypes)EditorGUILayout.EnumPopup("Git options", settings.GitOptions);

                GUIStyle style = new GUIStyle(EditorStyles.textField);
                if (!File.Exists(settings.WakatimeCliBinary))
                    style.normal.textColor = Color.red;

                settings.WakatimeCliBinary = EditorGUILayout.TextField("Wakatime CLI", settings.WakatimeCliBinary, style);

                EditorGUILayout.BeginHorizontal();
                settings.ApiKey = EditorGUILayout.TextField("API key", settings.ApiKey);
                if (GUILayout.Button("Get key"))
                    Application.OpenURL("https://wakatime.com/api-key");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                settings.ApiUri = EditorGUILayout.TextField("API URI", settings.ApiUri);
                // EditorGUILayout.LabelField("Default: https://api.wakatime.com/api/v1/");
                EditorGUILayout.EndHorizontal();

                //settings.ClientOptions = (ClientTypes)EditorGUILayout.EnumPopup("Client options", settings.ClientOptions);

                if (GUILayout.Button("Open dashboard"))
                    Application.OpenURL("https://wakatime.com/dashboard");

                var halfWidth = (EditorGUIUtility.currentViewWidth - 10) / 2;
                EditorGUILayout.BeginHorizontal();
                bool reset = GUILayout.Button("Reset To Default", GUILayout.Width(halfWidth));
                bool useEnv = GUILayout.Button("Use ~/wakatime.cfg", GUILayout.Width(halfWidth));
                EditorGUILayout.EndHorizontal();
                if (reset)
                {
                    settings.RestoreDefaults();
                }
                if (useEnv)
                {
                    settings.TryUseEnvConfig();
                }

                EditorGUILayout.BeginHorizontal();
                bool save = GUILayout.Button("Save preferences", GUILayout.Width(halfWidth));
                bool cancel = GUILayout.Button("Cancel", GUILayout.Width(halfWidth));
                EditorGUILayout.EndHorizontal();

                if (save)
                {
                    Plugin.SettingsManager.SaveSettings(settings);
                    Plugin.Initialize();
                    this.Close();
                }
                if (cancel)
                {
                    this.Close();
                }
            }
        }
    }
}


#endif
