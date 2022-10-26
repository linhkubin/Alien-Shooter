using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAutoFire : MonoBehaviour
{
    private Transform tf_this;
    public float health;
    void Start()
    {
        tf_this = transform;
    }

    public void EnemyMove(Transform pos)
    {
        //transform.position = Vector3.MoveTowards(tf_this.position, pos.position, 1 * Time.deltaTime);
        tf_this.Translate(Vector3.down * EnemyManager_R1_AutoFire.Ins.speedMove * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG) || collision.CompareTag(Constant.BULLET_BLUE_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }
        if (collision.CompareTag(Constant.BULLET_BIG_TAG))
        {
            TakeDamage2(999);
        }

        if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            TakeDamage(999);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            GameManager_R1.Ins.Coin += 40;
            EnemyManager_R1_AutoFire.Ins.PlayExplosionSound();
            DestroyShip();
        }
    }
    public virtual void TakeDamage2(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            GameManager_R1.Ins.Coin += 40;
            DestroyShip();
        }
    }

    public virtual void DestroyShip()
    {
        if (EnemyManager_R1_AutoFire.Ins.GetDiedEnemy() == 20)
        {
            EnemyManager_R1_AutoFire.Ins.UpdateHealth(12);
            EnemyManager_R1_AutoFire.Ins.speedMove = 2.2f;
        }

        if (!EnemyManager_R1_AutoFire.Ins.spawnedBooster_3 && EnemyManager_R1_AutoFire.Ins.GetDiedEnemy() == 1)
        {
            Instantiate(EnemyManager_R1_AutoFire.Ins.bulletPrefabs, transform.position, Quaternion.identity);
            EnemyManager_R1_AutoFire.Ins.spawnedBooster_3 = true;
        }
        else if (!EnemyManager_R1_AutoFire.Ins.spawnedBooster_5 && EnemyManager_R1_AutoFire.Ins.GetDiedEnemy() == 2)
        {
            Instantiate(EnemyManager_R1_AutoFire.Ins.boosterPrefabs, transform.position, Quaternion.identity);
            EnemyManager_R1_AutoFire.Ins.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);

        Destroy(gameObject);

        //if (EnemyManager_R1_AutoFire.Ins.countCoin < 25)
        //{
        //    float tt = UnityEngine.Random.Range(-1, 1) * 0.05f;

        //    Vector3 vv = new Vector3(transform.position.x + tt, transform.position.y, transform.position.z);

        //    ObjectPooling.Ins.SpawnFromPool(Constant.COIN_TAG, vv, Quaternion.identity);

        //    EnemyManager_R1_AutoFire.Ins.countCoin++;
        //}
    }

    public void autoDestroy(Transform pos)
    {
        if (transform.position.y < -13)
        {
            TakeDamage2(999);
        }
    }
}
