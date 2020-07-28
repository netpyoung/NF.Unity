using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NFRuntime.Util.Components
{
    public class WaitForButton : CustomYieldInstruction
    {
        private Button Button { get; }
        private readonly IEnumerator mRunner;

        public WaitForButton(Button btn, IEnumerator runner)
        {
            btn.interactable = false;
            this.Button = btn;
            this.mRunner = runner;
        }

        public override bool keepWaiting
        {
            get
            {
                bool isWaiting = this.mRunner.MoveNext();
                if (!isWaiting)
                {
                    this.Button.interactable = true;
                }
                return isWaiting;
            }
        }
    }
}