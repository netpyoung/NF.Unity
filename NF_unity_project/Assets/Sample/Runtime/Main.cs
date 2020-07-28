using Sample.Common;
using UnityEngine;

namespace Sample.Runtime
{
    public class Main : MonoBehaviour
    {
        async void Start()
        {
            App.sm.RegisterReporter<LoadingReporter>();
            SG_A asg = await App.sm.LoadAsync<SG_A>();
            SG_B bsg = await App.sm.LoadAsync<SG_B>();
        }
    }
}