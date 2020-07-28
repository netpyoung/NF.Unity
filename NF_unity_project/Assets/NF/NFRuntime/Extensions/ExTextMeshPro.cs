using UnityEngine;

namespace NFRuntime.Extensions
{
    public static class ExTextMeshPro
    {
        public static void SetText<T>(this TextMesh text, T t)
        {
            text.text = t.ToString();
        }
    }
}