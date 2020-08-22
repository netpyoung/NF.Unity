using NF.Mathematics;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    int mID = 0;
    int mSpeedBase = 3;
    int mSpeedAcceleration = 7;
    int mSpeedMax = 10;
    float mLifetimeAcc = 0;
    float mLifetimeMax = 10;
    int mHitMax = 2;
    private int mTargetLayer;
    Int3 mCollisionHalfExtents;
}

public class ShootInfo
{
    int ID;

    int Angle;
    int Count;

    public GameObject PrefabProjectile;
    public GameObject PrefabShootBegin;
    public GameObject PrefabShootHit;
    public GameObject PrefabShootEnd;
}


public class GoClientProjectile : MonoBehaviour
{
    public delegate void DelDispose(GoClientProjectile p);

    public DelDispose OnDispose;

    RaycastHit[] results = new RaycastHit[100];

    int mID = 0;
    int mSpeedBase = 3;
    int mSpeedAcceleration = 7;
    int mSpeedMax = 10;
    float mLifetimeAcc = 0;
    float mLifetimeMax = 10;
    int mHitMax = 2;
    private int mTargetLayer;
    Vector3 mCollisionHalfExtents = Vector3.one;

    float mSpeedCurr = 0;
    TrailRenderer TrailRenderer;
    HashSet<GameObject> mHittedGameObjects = new HashSet<GameObject>();


    void Awake()
    {
        mTargetLayer = LayerMask.GetMask("TARGET");

        TrailRenderer = transform.Find("Trail").GetComponent<TrailRenderer>();

    }

    public void Init()
    {
        TrailRenderer.emitting = true;
        TrailRenderer.Clear();

        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);

        this.mLifetimeAcc = 0;
        this.mSpeedCurr = mSpeedBase;
        this.mHittedGameObjects.Clear();
    }

    private void FixedUpdate()
    {
        if (mSpeedCurr != mSpeedMax)
        {
            mSpeedCurr = mSpeedCurr + mSpeedAcceleration * Time.fixedDeltaTime;
            mSpeedCurr = Math.Min(mSpeedCurr, mSpeedMax);
        }

        Vector3 currPos = this.transform.position;

        this.transform.Translate(Vector3.forward * mSpeedCurr * Time.fixedDeltaTime);

        int count = Physics.BoxCastNonAlloc(this.transform.position, mCollisionHalfExtents, Vector3.up, results, Quaternion.identity, Mathf.Infinity, mTargetLayer);
        if (count != 0)
        {
            Vector3 nextPos = this.transform.position;
            var dir = (nextPos - currPos).normalized;

            for (int i = 0; i < count; ++i)
            {
                var hitTarget = results[i].collider.gameObject;
                if (!mHittedGameObjects.Contains(hitTarget))
                {
                    mHittedGameObjects.Add(hitTarget);
                    if (mHittedGameObjects.Count <= mHitMax)
                    {
                        MonoBehaviour mono = hitTarget.GetComponent<MonoBehaviour>();
                        IProjectileHittable hittable = mono as IProjectileHittable;
                        if (hittable != null)
                        {
                            hittable.OnHit(new HitInfo { ID = mID, Dir = dir, PenetrateCount = mHittedGameObjects.Count });
                        }
                    }
                }
            }

            if (mHittedGameObjects.Count >= mHitMax)
            {
                Dispose();
            }
        }
        else
        {
            mLifetimeAcc += Time.deltaTime;
            if (mLifetimeAcc > mLifetimeMax)
            {
                Dispose();
            }
        }
    }

    void Dispose()
    {
        TrailRenderer.emitting = false;
        TrailRenderer.Clear();
        OnDispose(this);
    }
}
