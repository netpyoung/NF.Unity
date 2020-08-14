using UnityEngine;
using UnityEngine.UI;
using NFRuntime.Profile;

namespace NFRuntime.Util.Components
{
    public class FPSDisplay : MonoBehaviour
    {
        [System.Serializable]
        private struct FPSColor
        {
#pragma warning disable 0649
            public Color color;
            public int minimumFPS;
#pragma warning restore 0649
        }

#pragma warning disable 0649
        [SerializeField]
        private FPSColor[] coloring;
#pragma warning restore 0649

        private FPSCounter fpsCounter = new FPSCounter();

        public Text highestFPSLabel;
        public Text averageFPSLabel;
        public Text lowestFPSLabel;

        void Update()
        {
            fpsCounter.Tick(Time.unscaledDeltaTime);

            Display(highestFPSLabel, fpsCounter.GetFPS_High());
            Display(averageFPSLabel, fpsCounter.GetFPS_Avg());
            Display(lowestFPSLabel, fpsCounter.GetFPS_Low());
        }

        void Display(Text label, (int fps, string fpsStr) t)
        {
            label.text = t.fpsStr;
            for (int i = 0; i < coloring.Length; i++)
            {
                if (t.fps >= coloring[i].minimumFPS)
                {
                    label.color = coloring[i].color;
                    break;
                }
            }
        }
    }
}