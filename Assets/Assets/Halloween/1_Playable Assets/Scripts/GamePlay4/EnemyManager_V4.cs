using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_V4 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_V4 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject boosterPrefabs;
    public GameObject bulletPrefabs;

    public GameObject bulletEnemyPrefabs;

    public Transform leftPos, rightPos;

    public bool spawnedBooster_3 = false;
    public bool spawnedBooster_5 = false;


    public Enemy_V4[] enemiesBorder;
    public Enemy_V4[] enemiesInside;
    public int enemyCount;

    public Transform[] borderPos;
    public Transform[] insidePos;

    private float speedBorder = 0.75f;
    private float speedInside = 0.5f;

    void Start()
    {
        enemyCount = enemiesBorder.Length + enemiesInside.Length;

        SetupEnemyInitPos();

        StartCoroutine(IE_MoveEnemyBorder());
        StartCoroutine(IE_MoveEnemyInside());
    }

    void FixedUpdate()
	{
        if (GetDiedEnemy() == enemyCount && !GameManager_V4.Ins.isWin)
        {
            GameManager_V4.Ins.isWin = true;
            EndCard_V4.Ins.SetupEndCard();
            EndCard_V4.Ins.WaitToShowEndCard(1.5f);
        }
    }

    public void StartShooting()
	{
        foreach(Enemy_V4 e in enemiesBorder)
		{
            e.StartShooting();
		}

        foreach (Enemy_V4 e in enemiesInside)
        {
            e.StartShooting();
        }
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        for (int i = 0; i < enemiesBorder.Length; i++)
        {
            if (enemiesBorder[i] == null)
            {
                numDie++;
            }
        }

        for (int i = 0; i < enemiesInside.Length; i++)
        {
            if (enemiesInside[i] == null)
            {
                numDie++;
            }
        }
        return numDie;
    }

    public void SetupEnemyInitPos()
	{
        for (int i = 0; i < enemiesBorder.Length; i++)
        {
            enemiesBorder[i].curMove += i;
        }

        for (int i = 0; i < enemiesInside.Length; i++)
        {
            enemiesInside[i].curMove += i;
        }
    }

    public void MoveEnemyBorder()
	{
        for(int i = 0; i < enemiesBorder.Length; i++)
		{
            if (enemiesBorder[i] != null)
            {
                enemiesBorder[i].curMove += 1;
                int nextMove = enemiesBorder[i].curMove % enemiesBorder.Length;

                enemiesBorder[i].MoveToPos(borderPos[nextMove], speedBorder);
            }
            else continue;
		}
	}

    public void MoveEnemyInside()
    {
        for (int i = 0; i < enemiesInside.Length; i++)
        {
            if (enemiesInside[i] != null)
            {
                enemiesInside[i].curMove += 1;
                int nextMove = enemiesInside[i].curMove % enemiesInside.Length;

                enemiesInside[i].MoveToPos(insidePos[nextMove], speedInside);
            }
            else continue;
        }
    }

    IEnumerator IE_MoveEnemyBorder()
	{
        yield return new WaitForSeconds(1f);
        MoveEnemyBorder();
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(IE_MoveEnemyBorder());
    }

    IEnumerator IE_MoveEnemyInside()
    {
        yield return new WaitForSeconds(1f);
        MoveEnemyInside();
        yield return new WaitForSeconds(1f);
        StartCoroutine(IE_MoveEnemyInside());
    }
}
