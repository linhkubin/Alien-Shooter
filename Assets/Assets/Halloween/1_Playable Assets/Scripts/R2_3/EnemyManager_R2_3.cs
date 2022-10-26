using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_R2_3 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_R2_3 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject bulletPrefabs;

    //=================== ROUND 1 ==================
    [HideInInspector]
    public int numEnemy;

    public Enemy_R2_3[] enemiesAll;
    public Enemy_R2_3[] enemy1;
    public Enemy_R2_3[] enemy2;
    public Enemy_R2_3[] enemy3;
    public Enemy_R2_3[] enemy4;

    //=================== ROUND 2 ====================
    [HideInInspector]
    public Transform enemyContainer;
    public Enemy_R1_1[] enemies;
    public Enemy_R1_1[] leftEnemies;
    public Enemy_R1_1[] rightEnemies;
    public Transform leftPos, rightPos;

    public bool spawnedBooster_3 = false;
    public bool spawnedBooster_5 = false;


    void Start()
    {
        numEnemy = enemiesAll.Length;

        SetupEnemy();
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    public void SetupEnemy()
	{
        StartCoroutine(WaitSetupEnemy(enemy1, 0));
        StartCoroutine(WaitSetupEnemy(enemy2, 0.5f));
        StartCoroutine(WaitSetupEnemy(enemy3, 1f));
        StartCoroutine(WaitSetupEnemy(enemy4, 1.5f));
    }

    private IEnumerator WaitSetupEnemy(Enemy_R2_3[] enemy, float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].MoveToPos();
        }
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (Enemy_R2_3 e in enemiesAll)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    public void UpdateHealth(int health)
    {
        foreach (Enemy_R2_3 e in enemiesAll)
        {
            if (e != null)
            {
                e.health = health;
            }
        }
    }

    public void StartShooting()
    {
        foreach (Enemy_R2_3 e in enemiesAll)
        {
            e.StartShooting();
        }
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
