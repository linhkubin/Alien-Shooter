using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CacheCollider
{
    private static Dictionary<Collider, Ply_Enemy> cacheEnemy = new Dictionary<Collider, Ply_Enemy>();

    public static Ply_Enemy GetEnemy(Collider collider)
    {
        if (!cacheEnemy.ContainsKey(collider))
        {
            cacheEnemy.Add(collider, collider.GetComponent<Ply_Enemy>());
        }

        return cacheEnemy[collider];
    }
}
