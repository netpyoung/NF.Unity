using UnityEngine;

namespace NFRuntime.Util.Components
{
    [DisallowMultipleComponent]
    public class WaitForParticle : CustomYieldInstruction
    {
        public ParticleSystem Particle { get; }

        public WaitForParticle(ParticleSystem ps)
        {
            this.Particle = ps;
        }

        public override bool keepWaiting => this.Particle.IsAlive(true);
    }
}