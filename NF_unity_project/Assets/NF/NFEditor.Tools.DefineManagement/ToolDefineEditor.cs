using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace NFEditor.Tools.DefineManagement
{
    public class ToolDefineEditor<T> : EditorWindow where T : Enum
    {
        private static List<string> mPreDefines = new List<string>();

        private bool mIsChanged;
        private int mCurSelectedIndex;

        private readonly HashSet<string> mDefines = new HashSet<string>();
        private readonly List<string> mErrors = new List<string>();

        private string mFilterStr = string.Empty;
        private bool mIsLoaded;
        private Vector2 mScrollPos;

        public static void OpenWindow<TSelf>() where TSelf : ToolDefineEditor<T>
        {
            GetWindow<TSelf>(true);
        }

        public static void Init(List<string> pre_defines)
        {
            mPreDefines = pre_defines;
        }

        public static void ApplyDefines(params T[] defines)
        {
            var defs = string.Join(";", defines.Select(x => x.ToString()).ToArray());

            foreach (BuildTargetGroup type in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                switch (type)
                {
                    case BuildTargetGroup.Android:
                    case BuildTargetGroup.iOS:
                    case BuildTargetGroup.Standalone:
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(type, defs);
                        break;
                    default:
                        break;
                }
            }
        }

        private void OnEnable()
        {
            this.titleContent = new GUIContent("Script Defines");

            if (!this.mIsLoaded)
            {
                this.mIsLoaded = true;
                Reload();
            }

            Repaint();
        }

        private string[] GetFilteredDefines(List<string> defines, string filter_str)
        {
            if (string.IsNullOrEmpty(filter_str))
            {
                return defines.ToArray();
            }

            return defines.FindAll(x => { return x.Contains(filter_str); }).ToArray();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(EditorApplication.isCompiling ? "Compiling..." : " ");
            
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Reload"))
            {
                Reload();
            }

            GUI.enabled = this.mIsChanged && this.mErrors.Count == 0 && !EditorApplication.isCompiling;

            if (GUILayout.Button("Apply"))
            {
                Apply();
                Reload();
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            this.mScrollPos = EditorGUILayout.BeginScrollView(this.mScrollPos);

            var changed = false;
            EditorGUI.BeginChangeCheck();
            var buffer = this.mDefines.ToList();

            for (var i = 0; i < buffer.Count; ++i)
            {
                var define = buffer.ElementAt(i);
                GUILayout.BeginHorizontal();
                GUILayout.Label(define);

                if (GUILayout.Button("X"))
                {
                    this.mDefines.Remove(define);
                }

                GUILayout.EndHorizontal();
            }

            changed |= EditorGUI.EndChangeCheck();

            GUILayout.Space(24);
            var filtered_defines = GetFilteredDefines(mPreDefines, this.mFilterStr);

            this.mCurSelectedIndex = EditorGUILayout.Popup(this.mCurSelectedIndex, filtered_defines.ToArray());

            this.mFilterStr = EditorGUILayout.TextField(this.mFilterStr);

            if (filtered_defines.Length != 0)
            {
                if (GUILayout.Button("Add"))
                {
                    this.mDefines.Add(filtered_defines.ElementAt(this.mCurSelectedIndex));
                    changed = true;
                }
            }

            GUI.enabled = true;

            GUILayout.Space(50);
            EditorGUILayout.EndScrollView();
            GUILayout.Label("Project Settings > Player > Other Settings > Configuration > Script Define Symbols");

            if (this.mErrors.Count != 0)
            {
                GUILayout.Label("ERRORS:");
                var errorStyle = new GUIStyle(GUI.skin.label);
                errorStyle.normal.textColor = Color.red;

                for (var i = 0; i < this.mErrors.Count; i++)
                {
                    var error = this.mErrors[i];
                    GUILayout.Label(error, errorStyle);
                }
            }

            //		if(changed)
            //		{
            //			TestForErrors();
            //		}
            this.mIsChanged |= changed;
        }

        private void TestForErrors()
        {
            var containsInvalidRegex = new Regex("^[a-zA-Z0-9_]*$");
            var isNumberRegex = new Regex("^[0-9]*$");

            var containsInvalid = false;
            var isFirstNumber = false;
            var isEmptyString = false;

            foreach (var define in this.mDefines)
            {
                if (define.Length == 0)
                {
                    isEmptyString = true;
                }

                if (!containsInvalidRegex.IsMatch(define))
                {
                    containsInvalid = true;
                }

                if (define.Length != 0)
                {
                    if (isNumberRegex.IsMatch(define[0].ToString()))
                    {
                        isFirstNumber = true;
                    }
                }
            }

            this.mErrors.Clear();

            if (containsInvalid)
            {
                this.mErrors.Add("A script define contains invalid characters.");
            }

            if (isEmptyString)
            {
                this.mErrors.Add("A script define is empty.");
            }

            if (isFirstNumber)
            {
                this.mErrors.Add("A number cannot be used as the first character of a define.");
            }
        }

        private void Reload()
        {
            this.mIsChanged = false;
            this.mDefines.Clear();

            var defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            // var split = defines.Split(';');
            foreach (var define in defines.Split(';'))
            {
                if (string.IsNullOrEmpty(define))
                {
                    continue;
                }

                this.mDefines.Add(define);
            }

            EditorGUIUtility.editingTextField = false;
        }

        private void Apply()
        {
            var defines = string.Empty;

            if (this.mDefines.Count == 0)
            {
            }
            else if (this.mDefines.Count == 1)
            {
                defines = this.mDefines.First();
            }
            else
            {
                defines = this.mDefines.Aggregate((cur, nxt) => cur + ";" + nxt);
            }

            foreach (BuildTargetGroup type in Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (type == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(type, defines);
            }
        }
    }
}