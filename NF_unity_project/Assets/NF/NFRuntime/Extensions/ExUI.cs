using NFRuntime.Util.Components;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NFRuntime.Extensions
{
    public static class ExUI
    {
        public static void SetText(this Text text, object obj)
        {
            text.text = obj.ToString();
        }

        public static void SetText(this Button button, object obj)
        {
            Text text = button.FindGetComp<Text>("Text");
            text.SetText(obj);
        }

        public static void OnClick(this Image img, UnityAction func)
        {
            EventTrigger trigger = img.FGetComp<EventTrigger>();
            trigger.triggers.Clear();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(eventData => { func(); });
            trigger.triggers.Add(entry);
        }

        public static void OnClick(this GameObject go, UnityAction func)
        {
            EventTrigger trigger = go.FindGetComp<EventTrigger>();
            trigger.triggers.Clear();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(eventData => { func(); });
            trigger.triggers.Add(entry);
        }

        public static void OnClick(this Button btn, UnityAction func)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                // TODO(pyoung): prevent double click.
                using (btn.Using())
                {
                    func();
                }
            });
        }

        public static void OnClick_co(this Button btn, Func<IEnumerator> ieFunc)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { btn.StartCoroutine(new WaitForButton(btn, ieFunc())); });
        }

        public static void OnClick(this Toggle toggle, UnityAction<bool> func)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(func);
        }

        public static void OnClick(this Slider self, UnityAction<float> func)
        {
            self.OnValue(func);
        }

        public static void OnValue(this Slider self, UnityAction<float> func)
        {
            self.onValueChanged.RemoveAllListeners();
            self.onValueChanged.AddListener(func);
        }

        public static ButtonWrapper Using(this Button btn)
        {
            return new ButtonWrapper(btn);
        }

        public static void SetSize(this Image image, Vector2 size)
        {
            image.rectTransform.sizeDelta = size;
        }
    }
}