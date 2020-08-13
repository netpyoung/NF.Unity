using UnityEngine;
using UnityEditor;
using System.Linq;

namespace NFEditor.Tools.FileRevealer
{
    // ref: https://assetstore.unity.com/packages/tools/utilities/finder-explorer-revealer-74168

    [InitializeOnLoad]
    public static class ToolFileRevealer
    {
        static GUIContent mSearchIcon;
        static GUIContent mDarkSearchIcon;

        static ToolFileRevealer()
        {
            LoadIcon();

            EditorApplication.projectWindowItemOnGUI += AddRevealerIcon;
        }

        static void LoadIcon()
        {
            // ref: https://github.com/halak/unity-editor-icons

            mSearchIcon = EditorGUIUtility.IconContent("Search Icon");
            mDarkSearchIcon = EditorGUIUtility.IconContent("d_Search Icon");
        }

        static void AddRevealerIcon(string guid, Rect rect)
        {
            var isHover = rect.Contains(Event.current.mousePosition) && ToolFileRevealerSetting.ShowOnHover;
            var isSelected = IsSelected(guid) && ToolFileRevealerSetting.ShowOnSelected;

            bool isVisible = isHover || isSelected;

            if (!isVisible)
            {
                return;
            }

            float iconSize = EditorGUIUtility.singleLineHeight;
            const int Offset = 1;
            var iconRect = new Rect(
                rect.width + rect.x - iconSize - ToolFileRevealerSetting.OffsetInProjectView,
                rect.y,
                iconSize - Offset,
                iconSize - Offset
            );

            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (GUI.Button(iconRect, GetIcon(), GUIStyle.none))
            {
                EditorUtility.RevealInFinder(path);
            }

            EditorApplication.RepaintProjectWindow();
        }

        static GUIContent GetIcon()
        {
            if (mDarkSearchIcon == null || mSearchIcon == null)
            {
                LoadIcon();
            }

            return EditorGUIUtility.isProSkin ? mDarkSearchIcon : mSearchIcon; ;
        }

        static bool IsSelected(string guid)
        {
            return Selection.assetGUIDs.Any(guid.Contains);
        }
    }
}