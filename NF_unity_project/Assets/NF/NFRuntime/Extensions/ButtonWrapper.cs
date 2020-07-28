using System;
using UnityEngine.UI;

namespace NFRuntime.Extensions
{
    public class ButtonWrapper : IDisposable
    {
        private Button btn;

        public ButtonWrapper(Button btn)
        {
            btn.interactable = false;
            this.btn = btn;
        }

        public void Dispose()
        {
            this.btn.interactable = true;
            this.btn = null;
        }
    }
}