using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_R2_3 : MonoBehaviour
{
    public Animator anim;

    public float health;
    public Transform pos;

    private EnemyManager_R2_3 enemyManager;

    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        enemyManager = EnemyManager_R2_3.Ins;
    }

    public void MoveToPos()
    {
        transform.DOMove(pos.position, 1);
    }


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
            enemyManager.PlayExplosionSound();
            DestroyShip();
        }
    }

    public void StartShooting()
    {
        StartCoroutine(IE_Shooting());
    }

    public void Shooting()
    {
        ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_BULLET_TAG, transform.position, Quaternion.identity);
    }

    public virtual void DestroyShip()
    {
        if (enemyManager.GetDiedEnemy() == 2)
        {
            enemyManager.UpdateHealth(2);
        }

        if (!enemyManager.spawnedBooster_3 && enemyManager.GetDiedEnemy() == 1)
        {
            Instantiate(enemyManager.bulletPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_3 = true;
        }
        else if (!enemyManager.spawnedBooster_5 && enemyManager.GetDiedEnemy() == 6)
        {
            Instantiate(enemyManager.bulletPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.8f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
