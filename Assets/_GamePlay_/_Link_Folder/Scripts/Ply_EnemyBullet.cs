using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_EnemyBullet : Ply_GameUnit
{
    float speed = -5f;

    public void OnInit()
    {
        //set dau vao cho bullet
    }

    public void OnDespawn()
    {
        Ply_Pool.Ins.Despawn(PoolType.Enemy_Bullet, this);
    }

    // Update is called once per frame
    void Update()
    {
        tf.Translate(tf.up * Time.deltaTime * speed, Space.Self);

        if (tf.position.y < -12f)
        {
            OnDespawn();
        }
    }

}
