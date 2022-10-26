using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip_V2 : MonoBehaviour
{
    public bool isDie = false;
    public bool haveBooster;
    protected float health;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }

        if (collision.CompareTag(Constant.PLAYER_TAG))
		{
            TakeDamage(10);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_V2.Ins.PlayExplosionSound();
            DestroyShip();
        }
    }

    public virtual void DestroyShip()
    {
        float rand = Random.Range(0, 1f);
        if (!EnemyManager_V2.Ins.spawnedBooster && haveBooster)
        {
            Instantiate(EnemyManager_V2.Ins.boosterPrefabs, transform.position, Quaternion.identity);
            EnemyManager_V2.Ins.spawnedBooster = true;
        }
        else
        {
            if (rand < 0.1f)
            {
                ObjectPooling.Ins.SpawnFromPool(Constant.COIN_TAG, transform.position, Quaternion.identity);
            }
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
