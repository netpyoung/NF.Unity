using NFRuntime.ObjectPool;
using UnityEditor;

namespace NFEditor.ObjectPool
{
    [CustomEditor(typeof(GameObjectPoolInspector))]
    public class GameObjectPoolInspectorEditor : Editor
    {
        GameObjectPoolInspector mInstance;

        public void OnEnable()
        {
            mInstance = target as GameObjectPoolInspector;
        }

        public override void OnInspectorGUI()
        {
            if (mInstance == null)
            {
                return;
            }

            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(nameof(mInstance.UsedSize));
                EditorGUILayout.IntField(mInstance.UsedSize);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(nameof(mInstance.CachedSize));
                EditorGUILayout.IntField(mInstance.CachedSize);
            }
            EditorGUILayout.EndHorizontal();
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}
