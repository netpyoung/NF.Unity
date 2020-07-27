using System.Threading;
using UnityEngine;

namespace NFRuntime.Managements.ResourceManagement.AssetBundleLoad
{
    public class AssetBundleContainer
    {
        public AssetBundle Bundle { get; set; }
        public int Reference => mReference;
        private int mReference = 1;

        public void Increment()
        {
            Interlocked.Increment(ref this.mReference);
        }

        public void Decrement()
        {
            Interlocked.Decrement(ref this.mReference);
        }
    }
}