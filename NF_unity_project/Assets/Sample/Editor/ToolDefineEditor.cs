using UnityEditor;
using System;
using System.Linq;

namespace Sample
{
    [InitializeOnLoad]
    public class ToolDefineEditor : NFEditor.Tools.DefineManagement.ToolDefineEditor<E_DEFINE>
    {
        static ToolDefineEditor()
        {
            var pre_defines = Enum.GetNames(typeof(E_DEFINE)).ToList();
            Init(pre_defines);
        }

        [MenuItem("@Tool/ToolDefineEditor")]
        private static void OpenScriptDefines()
        {
            OpenWindow<ToolDefineEditor>();
        }
    }
}
