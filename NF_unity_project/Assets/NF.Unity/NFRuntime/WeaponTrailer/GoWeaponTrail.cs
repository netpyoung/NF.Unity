using UnityEngine;

namespace NFRuntime.WeaponTrailer
{

    [RequireComponent(typeof(MeshRenderer))]
    public class GoWeaponTrail : MonoBehaviour
    {
        #region Public
        public TrailRecordConfig Config;
        #endregion Public

        TrailRecorder mRecorder = new TrailRecorder();
        TrailPlayer mPlayer = new TrailPlayer();
        TrailRecordInfo mInfo;

        void Start()
        {
            mInfo = PlayBegin();
        }

        void Update()
        {
            RecordAndPlay(mInfo, Time.deltaTime);
        }

        private void OnDisable()
        {
            mInfo.Dispose();
        }

        public TrailRecordInfo PlayBegin()
        {
            var mr = GetComponent<MeshRenderer>();
            Config.RegisterLayerInfo(gameObject.layer, mr.sortingLayerName, mr.sortingOrder);
            var recordInfo = new TrailRecordInfo(Config);
            mPlayer.Init(recordInfo);
            return recordInfo;
        }

        public void RecordAndPlay(TrailRecordInfo recordInfo, float dt)
        {
            mRecorder.Record(recordInfo, dt);
            mPlayer.Play(recordInfo, 0);
        }

        void OnDrawGizmos()
        {
            if (Config.TransformBase == null || Config.TransformTip == null)
            {
                return;
            }


            float dist = (Config.TransformBase.position - Config.TransformTip.position).magnitude;
            if (dist < Mathf.Epsilon)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Config.TransformBase.position, dist * 0.04f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Config.TransformTip.position, dist * 0.04f);
        }
    }
}
