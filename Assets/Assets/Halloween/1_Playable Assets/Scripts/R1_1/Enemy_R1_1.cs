using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_R1_1 : MonoBehaviour
{
    public Animator anim;

    public float health;
    public Transform pos;
    public bool isLeft;
    public bool canMove = false;
    public bool canTakeDamage = false;
    public Vector3 startPos;

    private EnemyManager_R1_Remake_2 enemyManager; 

    void Start()
    {
        enemyManager = EnemyManager_R1_Remake_2.Ins;
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }

    public void MoveToPos()
    {
        if (isLeft)
        {
            startPos = enemyManager.rightPos.position;
        }
        else
        {
            startPos = enemyManager.leftPos.position;
        }

        Vector3[] path = { startPos, pos.position };

        transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete(() => {

            canTakeDamage = true;
        });
    }


    public void OnTriggerEnter(Collider collision)
    {
        if(canTakeDamage == true)
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

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.9f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }

    public virtual void DestroyShip()
    {
        if (enemyManager.GetDiedEnemy() == 2)
        {
            //enemyManager.UpdateHealth(2);
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

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_GALAGA_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
