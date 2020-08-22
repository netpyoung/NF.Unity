using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoWall : MonoBehaviour, IProjectileHittable
{
    public void OnHit(HitInfo hitInfo)
    {
        Debug.Log($"Hitted{gameObject.name}");
    }
}
