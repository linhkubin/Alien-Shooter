using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Bullet : Ply_GameUnit
{
    float speed = 20f;

    RaycastHit hit;

    public LayerMask enemyLayer;

    //TODO: Note set up type dau vao
    public void OnInit()
    {
        //set dau vao cho bullet
    }

    public void OnDespawn()
    {
        Ply_Pool.Ins.Despawn(PoolType.Bullet, this);
    }

    // Update is called once per frame
    void Update()
    {
        tf.Translate(tf.up * Time.deltaTime * speed, Space.Self);

        if (IsHitEnemy() || tf.position.y > 12f)
        {
            OnDespawn();
        }
    }

    private bool IsHitEnemy()
    {
        if (Physics.Raycast(tf.position, tf.up, out hit, Time.deltaTime * speed * 2.5f, enemyLayer))
        {
            //spawn vfx, damage
            Ply_Pool.Ins.Spawn(PoolType.VFX_Spark, hit.point, Quaternion.identity);
            CacheCollider.GetEnemy(hit.collider).TakeDamage(1);
            return true;
        }

        return false;
    }
}
