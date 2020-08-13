using System;
using UnityEditor;
using UnityEngine;

namespace NFEditor.Tools.FileRevealer
{
    public static class ToolFileRevealerSetting
    {
        static string ProjectName
        {
            get
            {
                var s = Application.dataPath.Split('/');
                var p = s[s.Length - 2];
                return p;
            }
        }

        public static EditorPrefsIntSlider OffsetInProjectView =
            new EditorPrefsIntSlider($"ToolFileRevealerSettings.OffsetInProjectView.{ProjectName}", "Offset in Project View", 0, 0, 100);
        public static EditorPrefsBool ShowOnHover =
            new EditorPrefsBool($"ToolFileRevealerSettings.ShowOnHover.{ProjectName}", "Show On Hovered Item", true);
        public static EditorPrefsBool ShowOnSelected =
            new EditorPrefsBool($"ToolFileRevealerSettings.ShowOnSelected.{ProjectName}", "Show On Selected Item", true);

        public abstract class EditorPrefsItem<T>
        {
            public string Key;
            public string Label;
            public T DefaultValue;

            public EditorPrefsItem(string key, string label, T defaultValue)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }

                Key = key;
                Label = label;
                DefaultValue = defaultValue;
            }

            public abstract T Value { get; set; }
            public abstract void Draw();

            public static implicit operator T(EditorPrefsItem<T> s)
            {
                return s.Value;
            }
        }

        public class EditorPrefsInt : EditorPrefsItem<int>
        {
            public EditorPrefsInt(string key, string label, int defaultValue)
                : base(key, label, defaultValue)
            {
            }

            public override int Value
            {
                get { return EditorPrefs.GetInt(Key, DefaultValue); }
                set { EditorPrefs.SetInt(Key, value); }
            }

            public override void Draw()
            {
                Value = EditorGUILayout.IntField(Label, Value);
            }
        }

        public class EditorPrefsIntSlider : EditorPrefsInt
        {
            readonly int mLeftValue;
            readonly int mRightValue;

            public EditorPrefsIntSlider(string key, string label, int defaultValue, int leftValue, int rightValue)
                : base(key, label, defaultValue)
            {
                mLeftValue = leftValue;
                mRightValue = rightValue;
            }

            public override void Draw()
            {
                Value = EditorGUILayout.IntSlider(Label, Value, mLeftValue, mRightValue);
            }
        }

        public class EditorPrefsString : EditorPrefsItem<string>
        {
            public EditorPrefsString(string key, string label, string defaultValue)
                : base(key, label, defaultValue)
            {
            }

            public override string Value
            {
                get { return EditorPrefs.GetString(Key, DefaultValue); }
                set { EditorPrefs.SetString(Key, value); }
            }

            public override void Draw()
            {
                Value = EditorGUILayout.TextField(Label, Value);
            }
        }

        public class EditorPrefsBool : EditorPrefsItem<bool>
        {
            public EditorPrefsBool(string key, string label, bool defaultValue)
                : base(key, label, defaultValue)
            {
            }

            public override bool Value
            {
                get { return EditorPrefs.GetBool(Key, DefaultValue); }
                set { EditorPrefs.SetBool(Key, value); }
            }

            public override void Draw()
            {
                Value = EditorGUILayout.Toggle(Label, Value);
            }
        }

        public class EditorPrefsColor : EditorPrefsItem<Color>
        {
            readonly string R;
            readonly string G;
            readonly string B;
            readonly string A;

            public EditorPrefsColor(string key, string label, Color defaultValue)
                : base(key, label, defaultValue)
            {
                R = $"{Key}_R";
                G = $"{Key}_G";
                B = $"{Key}_B";
                A = $"{Key}_A";
            }

            public override Color Value
            {
                get
                {
                    if (EditorPrefs.GetBool(Key, false))
                    {
                        return new Color(
                            EditorPrefs.GetFloat(R, 1),
                            EditorPrefs.GetFloat(G, 1),
                            EditorPrefs.GetFloat(B, 1),
                            EditorPrefs.GetFloat(A, 1));
                    }
                    else
                    {
                        return DefaultValue;
                    }
                }
                set
                {
                    EditorPrefs.SetBool(Key, true);
                    EditorPrefs.SetFloat(R, value.r);
                    EditorPrefs.SetFloat(G, value.g);
                    EditorPrefs.SetFloat(B, value.b);
                    EditorPrefs.SetFloat(A, value.a);
                }
            }

            public override void Draw()
            {
                Value = EditorGUILayout.ColorField(Label, Value);
            }
        }

        //[PreferenceItem("FileRevealer")]
        //public static void EditorPreferences()
        //{
        //    OffsetInProjectView.Draw();
        //    ShowOnHover.Draw();
        //    ShowOnSelected.Draw();

        //    GUILayout.FlexibleSpace();
        //    EditorGUILayout.LabelField("Version 1.0", EditorStyles.centeredGreyMiniLabel);
        //}


        [SettingsProvider]
        static SettingsProvider SettingsProvider()
        {
            return new ToolFileRevealerSettingProvider("Preferences/FileRevealer");
        }

        private class ToolFileRevealerSettingProvider : SettingsProvider
        {
            public ToolFileRevealerSettingProvider(string path, SettingsScope scopes = SettingsScope.User)
                : base(path, scopes)
            {
            }

            public override void OnGUI(string searchContext)
            {
                OffsetInProjectView.Draw();
                ShowOnHover.Draw();
                ShowOnSelected.Draw();
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Version 1.0", EditorStyles.centeredGreyMiniLabel);
            }
        }
    }
}