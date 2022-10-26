using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Booster : Ply_GameUnit
{
    float speed = -5f;
    float time;

    private void OnEnable()
    {
        time = 0.7f;
    }

    public void OnDespawn()
    {
        Ply_Pool.Ins.Despawn(PoolType.Enemy_Bullet, this);
    }

    void Update()
    {
        if (time > 0)
        {
            tf.Translate(tf.up * Time.deltaTime * speed, Space.Self);
            time -= Time.deltaTime;
        }
        else
        {
            tf.position = Vector3.MoveTowards(tf.position, Ply_Level.Ins.ship.tf.position, Time.deltaTime * 20f);

            if (Vector3.Distance(tf.position, Ply_Level.Ins.ship.tf.position) < 0.1f)
            {
                OnDespawn();
                Ply_Level.Ins.ship.LevelUp();
            }
        }
    }
}
