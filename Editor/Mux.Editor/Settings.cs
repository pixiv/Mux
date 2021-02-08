// Copyright 2019 pixiv Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.Build.Framework;
using UnityEditor;
using UnityEditor.SettingsManagement;

namespace Mux.Editor
{
    internal static class Settings
    {
        public readonly static UnityEditor.SettingsManagement.Settings Backend =
            new UnityEditor.SettingsManagement.Settings("com.pixiv.mux");

        [UserSetting("Editor", "Enable Live Reload", "With Live Reload, when you save your XAML file the changes are reflected live in your running app.")]
        public readonly static UserSetting<bool> EnableLiveReload =
            new UserSetting<bool>(Backend, "EnableLiveReload", true);

        [UserSetting("Build", "Verbosity", "The level of detail (i.e. verbosity) of an event log")]
        public readonly static UserSetting<LoggerVerbosity> Verbosity =
            new UserSetting<LoggerVerbosity>(Backend, "Verbosity", LoggerVerbosity.Normal);

        [UserSetting]
        public readonly static UserSetting<bool> OptimizeIL =
            new UserSetting<bool>(Backend, "OptimizeIL", true);

        [UserSetting]
        public readonly static UserSetting<bool> DebugSymbols =
            new UserSetting<bool>(Backend, "DebugSymbols", true);

        [UserSettingBlock("Build")]
        private static void XamlC(string search)
        {
            EditorGUI.BeginChangeCheck();

            using (new SettingsGUILayout.IndentedGroup("XamlC"))
            {
                OptimizeIL.value = SettingsGUILayout.SettingsToggle("Optimize IL", OptimizeIL, search);
                DebugSymbols.value = SettingsGUILayout.SettingsToggle("Debug Symbols", DebugSymbols, search);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Backend.Save();
            }
        }

        [SettingsProvider]
        private static SettingsProvider CreateProvider()
        {
            return new UserSettingsProvider(
                "Preferences/Mux",
                Backend,
                new[] { typeof(Settings).Assembly }
            );
        }
    }
}
