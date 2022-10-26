using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_V4 : MonoBehaviour
{
    public float health;

    public Transform initPos;
    public bool canMove = false;

    [HideInInspector] public int curMove = 0;

    private EnemyManager_V4 enemyManager;

    void Start()
    {
        enemyManager = EnemyManager_V4.Ins;

        MoveToPos(initPos, 1f);
    }

    public void StartShooting()
	{
        StartCoroutine(IE_Shooting());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }

        else if (collision.CompareTag(Constant.PLAYER_TAG))
		{
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            enemyManager.PlayExplosionSound();
            DestroyShip();
        }
    }

    public void Shooting()
    {
        Instantiate(enemyManager.bulletEnemyPrefabs, transform.position, Quaternion.identity);
    }

    public void DestroyShip()
    {
        if (!enemyManager.spawnedBooster_3 && enemyManager.GetDiedEnemy() == 1)
        {
            Instantiate(enemyManager.bulletPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_3 = true;
        }
        else if (!enemyManager.spawnedBooster_5 && enemyManager.GetDiedEnemy() == 5) {
            Instantiate(enemyManager.boosterPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_5 = true;
        }
		
        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void MoveToPos(Transform pos, float moveDuration)
    {
        transform.DOMove(pos.position, moveDuration).SetEase(Ease.Linear);
    }

    IEnumerator IE_Shooting()
	{
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.5f)
		{
            Shooting();
		}

        StartCoroutine(IE_Shooting());
	}
}
