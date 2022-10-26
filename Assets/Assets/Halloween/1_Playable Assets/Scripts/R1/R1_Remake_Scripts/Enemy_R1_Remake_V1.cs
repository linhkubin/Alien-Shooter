using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy_R1_Remake_V1 : MonoBehaviour
{
    #region Singleton
    public static Enemy_R1_Remake_V1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject bulletEnemyPrefabs;

    public GameObject boosterPrefabs;
    public GameObject bulletPrefabs;

    public Transform leftPos, rightPos;

    public Transform enemyContainer;
    public Enemy_R1[] enemies;
    [HideInInspector]
    public int enemies_num;

    public Enemy_R1[] leftEnemies, rightEnemies;

    [HideInInspector]
    public bool spawnedBooster_3 = false;
    [HideInInspector]
    public bool spawnedBooster_5 = false;

    void Start()
    {
        enemies_num = enemies.Length;
        //EnemyStart();
        SetupEnemies(leftEnemies);
        SetupEnemies(rightEnemies);

        StartCoroutine(IE_MoveEnemyContainer());
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    public void EnemyStart()
    {
        foreach (Enemy_R1 e in enemies)
        {
            //e.canMove = true;
            e.MoveToPos();
        }
    }

    public void UpdateHealth(int health)
    {
        foreach (Enemy_R1 e in enemies)
        {
            if (e != null)
            {
                e.health = health;
            }
        }
    }

    public void StartShooting()
    {
        foreach (Enemy_R1 e in enemies)
        {
            e.StartShooting();
        }
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (Enemy_R1 e in enemies)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    private void SetupEnemies(Enemy_R1[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            StartCoroutine(IE_SetupEnemy(enemies[i], i / 10f));
        }
    }

    IEnumerator IE_SetupEnemy(Enemy_R1 e, float idx)
    {
        yield return new WaitForSeconds(0.5f + idx);
        e.MoveToPos();
    }


    IEnumerator IE_MoveEnemyContainer()
    {
        yield return new WaitForSeconds(5f);
        enemyContainer.DOMoveX(-1, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            enemyContainer.DOMoveX(1, 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                enemyContainer.DOMoveX(0, 2f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    StartCoroutine(IE_MoveEnemyContainer());
                });
            });
        });
    }
}
