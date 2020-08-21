using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NFEditor.Tools.PathManager
{
    public class ToolPathEditor : EditorWindow
    {
        [MenuItem("@Tool/ToolPathEditor")]
        public static void ShowExample()
        {
            ToolPathEditor wnd = GetWindow<ToolPathEditor>();
            wnd.titleContent = new GUIContent("ToolPathEditor");
        }

        public void OnEnable()
        {
            VisualElement root = rootVisualElement;
            var visualTree = Resources.Load<VisualTreeAsset>("ToolPathEditor");
            VisualElement labelFromUXML = visualTree.CloneTree();
            root.Add(labelFromUXML);

            root.Query<Button>(nameof(Application.dataPath)).First().clickable.clicked += () =>
            {
                Debug.Log(Application.dataPath);
                EditorUtility.RevealInFinder(Application.dataPath);
            };
            root.Query<Button>(nameof(Application.persistentDataPath)).First().clickable.clicked += () =>
            {
                Debug.Log(Application.persistentDataPath);
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            };
            root.Query<Button>(nameof(Application.streamingAssetsPath)).First().clickable.clicked += () =>
            {
                Debug.Log(Application.streamingAssetsPath);
                EditorUtility.RevealInFinder(Application.streamingAssetsPath);
            };
            root.Query<Button>(nameof(Application.temporaryCachePath)).First().clickable.clicked += () =>
            {
                Debug.Log(Application.temporaryCachePath);
                EditorUtility.RevealInFinder(Application.temporaryCachePath);
            };
            root.Query<Button>("log").First().clickable.clicked += () =>
            {
#if UNITY_EDITOR_WIN
                string dir =
                        $"{Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")}\\AppData\\Local\\Unity\\Editor";
#else
                string dir = $"{Environment.GetEnvironmentVariable("HOME")}/Library/Logs/Unity";
#endif
                Debug.Log(dir);
                EditorUtility.RevealInFinder(dir);
            };

            root.Query<Button>("nuget").First().clickable.clicked += () =>
            {
#if UNITY_EDITOR_WIN
                string dir =
                        $"{Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")}\\.nuget\\packages";
#else
                string dir = $"{Environment.GetEnvironmentVariable("HOME")}/.nuget/packages";
#endif
                Debug.Log(dir);
                EditorUtility.RevealInFinder(dir);
            };
        }
    }
}