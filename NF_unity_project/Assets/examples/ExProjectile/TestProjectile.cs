using NFRuntime.ObjectPool;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
public class TestProjectile : MonoBehaviour
{
    public Transform FireTransform;
    public GameObject Prefab;

    PrefabPool<GoClientProjectile> mPool;
    GoRangeMesh mRangeMesh;
    private void Awake()
    {
        mRangeMesh = transform.Find("GoRangeMesh").GetComponent<GoRangeMesh>();
    }
    void Start()
    {
        mRangeMesh.Init(60);

        PrefabPoolBuilder<GoClientProjectile> builder = new PrefabPoolBuilder<GoClientProjectile>();
        mPool = builder
            .Register(Prefab)
            .SetInitialCacheSize(10)
            .SetNumbering(true)
            .SetInspector(true)
            .Build();
        DoFire();
    }

    IEnumerable<float> GetAngles(int angle, int n)
    {
        if (n == 1)
        {
            yield return 0;
            yield break;
        }

        float halfAngle = angle / 2;
        yield return -halfAngle;
        yield return halfAngle;

        if (n == 2)
        {
            yield break;
        }

        int diviedAngle = angle / (n - 1);
        for (int i = 1; i <= n - 2; ++i)
        {
            yield return -halfAngle + i * diviedAngle;
        }
    }

    async void DoFire()
    {

        foreach (var angle in GetAngles(60, 3))
        {
            mPool.TryTake(out var projectile);
            projectile.Init();
            projectile.transform.localRotation = this.transform.rotation;
            projectile.transform.position = this.transform.position;
            projectile.transform.Rotate(Vector3.up, angle, Space.World);
            projectile.OnDispose = p => mPool.Return(p);
        }

        await Task.Delay(1000);

        foreach (var angle in GetAngles(60, 4))
        {
            mPool.TryTake(out var projectile);
            projectile.Init();
            projectile.transform.localRotation = this.transform.rotation;
            projectile.transform.position = this.transform.position;
            projectile.transform.Rotate(Vector3.up, angle, Space.World);
            projectile.OnDispose = p => mPool.Return(p);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            DoFire();
        }
    }
}
