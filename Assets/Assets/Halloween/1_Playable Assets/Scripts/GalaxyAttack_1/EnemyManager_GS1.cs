using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luna.Unity;

public class EnemyManager_GS1 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_GS1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject boosterPrefabs;
    public GameObject shipPrefabs;

    //=================== ROUND 1 ==================
    [HideInInspector]
    public int numGalaEnemy;
    public GalaEnemy_GS1[] rectangle;
    public Enemy_GS1[] star;
    public GalaEnemy_GS1 boss;
    private float speedInitRound1 = 1f;

    public Transform[] rectanglePos;
    public Transform[] starPos;

    private float speedTriangleLeft = 1.5f;
    private float speedTriangleRight = 0.5f;
    private float speedRectangle = 1.5f;
    private float speedStar = 1.5f;

    [SerializeField]
    private bool isRound1 = true;

    //=================== ROUND 2 ====================
    [HideInInspector]
    public int numEnemy;
    public Transform posLeft, posRight;
    public EnemyR1Pizza[] enemies;
        
    public bool spawnedShip = false;
    public bool spawnedBooster = false;

    [SerializeField]
    private bool isRound2 = false;

    //=================== ROUND 3 ====================
    [HideInInspector]
    public int numEnemy3;
    public EnemyRoam_GS1[] enemiesRoam;
    public EnemyR2V3MoveRound2[] enemiesR2V3;
    [SerializeField]
    private bool isRound3 = false;

    //=================== ENDCARD ====================
    //public Transform bossEndCard;

    public EnemyEndcard[] enemy1;
    public EnemyEndcard[] enemy2;
    public EnemyEndcard[] enemy3;

    public EnemyEndCardR1V1190722[] enemiesRound2;

    public EnemyR2V3MoveRound2[] enemyRound21;
    public EnemyR2V3MoveRound2[] enemyRound22;
    public EnemyR2V3MoveRound2[] enemyRound23;
    public EnemyR2V3MoveRound2[] enemyRound24;
    public EnemyR2V3MoveRound2[] enemyRound25;

    void Start()
    {
        numGalaEnemy = rectangle.Length + 1;
        numEnemy = enemies.Length;
        //numEnemy3 = enemiesRoam.Length;
        numEnemy3 = enemiesR2V3.Length;
        //SetupEnemyRound1();
        Analytics.LogEvent(Analytics.EventType.LevelStart, 1);
    }

    private void FixedUpdate()
    {
        if (isRound1)
        {
            if (GetDiedGalaEnemy() == numGalaEnemy)
            {
                isRound1 = false;
                //Setup Round2

                isRound2 = true;
                SetupEnemyRound2();
            }
        }

        else if (isRound2)
        {
            if (GetDiedEnemy() == numEnemy)
            {
                isRound2 = false;

                GameManager_GS1.Ins.ShowWarning(true);
                //Invoke(nameof(startEnemyRoam), 1.5f);
                Invoke(nameof(SetupEnemyR2Round2), 1.5f);
                //Invoke(nameof(DelayTakeDame), 4.15f);

                isRound3 = true;
                Analytics.LogEvent(Analytics.EventType.LevelWon, 1);
            }
        }
        else if (isRound3)
        {
            if (GetDiedEnemyRound2() == numEnemy3)
            {
                // Setup EndCard
                Player_GS1.Ins.WaitToTurnOffBullet(0f);
                //WaitToShowBoss(0.25f);
                Invoke(nameof(setUpEnemyEndcard), 0.5f);
                isRound3 = false;
                Analytics.LogEvent(Analytics.EventType.LevelWon, 2);
                Analytics.LogEvent(Analytics.EventType.EndCardShown, 1);
            }
        }
    }
    public void setUpEnemyEndcard()
    {
        foreach (EnemyEndCardR1V1190722 e in enemiesRound2)
        {
            e.MoveToPos();
        }
    }
    public void DelayTakeDame()
    {
        foreach (EnemyRoam_GS1 e in enemiesRoam)
        {
            e.canTakeDamage = true;
        }
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    #region ROUND_1
    private void SetupEnemyRound1()
    {
        //for (int i = 0; i < rectangle.Length; i++)
        //{
        //    rectangle[i].targerPos = rectanglePos[i].transform;
        //}

        //for (int i = 0; i < rectangle.Length; i++)
        //{
        //    rectangle[i].MoveToInitPos();
        //    rectangle[i].curMove += i;
        //    rectangle[i].StartShooting();
        //}
        //boss.MoveToInitPos();

        //Invoke(nameof(StartMoveEnemy), 1f);
    }

    private void StartMoveEnemy()
    {
        //StartCoroutine(IE_MoveEnemyRound1(rectangle, rectanglePos, speedRectangle, 0.75f, 2f));
    }
    private void StartMoveEnemyStar()
    {
        StartCoroutine(IE_MoveEnemyStar(star, starPos, speedStar, 0.5f, 1.3f));
    }

    //private void MoveEnemyRound1(GalaEnemy_GS1[] enemies, Transform[] positions, float speed)
    //{
    //    for (int i = 0; i < enemies.Length; i++)
    //    {
    //        if (enemies[i] != null)
    //        {
    //            enemies[i].curMove += 1;
    //            int nextMove = enemies[i].curMove % enemies.Length;

    //            enemies[i].MoveToPos(positions[nextMove], speed);
    //        }
    //        else continue;
    //    }
    //}
    private void MoveEnemyStar(Enemy_GS1[] enemies, Transform[] positions, float speed)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].curMove += 1;
                int nextMove = enemies[i].curMove % enemies.Length;

                enemies[i].MoveAroundStar(positions[nextMove], speed);
            }
            else continue;
        }
    }

    //IEnumerator IE_MoveEnemyRound1(GalaEnemy_GS1[] enemies, Transform[] positions, float speed, float time1, float time2)
    //{
    //    yield return new WaitForSeconds(time1);
    //    MoveEnemyRound1(enemies, positions, speed);
    //    yield return new WaitForSeconds(time2);
    //    StartCoroutine(IE_MoveEnemyRound1(enemies, positions, speed, time1, time2));
    //}
    IEnumerator IE_MoveEnemyStar(Enemy_GS1[] enemies, Transform[] positions, float speed, float time1, float time2)
    {
        yield return new WaitForSeconds(time1);
        MoveEnemyStar(enemies, positions, speed);
        yield return new WaitForSeconds(time2);
        StartCoroutine(IE_MoveEnemyStar(enemies, positions, speed, time1, time2));
    }

    public int GetDiedGalaEnemy()
    {
        int numDie = 0;
        for (int i = 0; i < rectangle.Length; i++)
        {
            if (rectangle[i] == null) {
                numDie++; 
            }
        }
        if (boss == null) numDie++;
        return numDie;
    }
    #endregion

    #region ROUND_2
    private void SetupEnemyRound2()
    {
        foreach (EnemyR1Pizza e in enemies)
        {
            e.MoveToPos();
            e.StartShooting();
        }

        for(int i = 0; i < star.Length; i++)
        {
            star[i].curMove += i;
        }
        Invoke(nameof(StartMoveEnemyStar), 1f);
    }

    public void SpawnShipBoost(Transform pos)
	{
        Instantiate(shipPrefabs, pos.position, Quaternion.identity);
	}

    public void SpawnBooster(Transform pos)
    {
        Instantiate(boosterPrefabs, pos.position, Quaternion.identity);
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (EnemyR1Pizza e in enemies)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }
    #endregion

    #region ROUND_3

    public void startEnemyRoam()
    {
        StartCoroutine(IE_SetupEnemyRound3());

    }
    IEnumerator IE_SetupEnemyRound3()
    {
        GameManager_GS1.Ins.ShowWarning(false);
        foreach (EnemyRoam_GS1 e in enemiesRoam)
        {
            yield return new WaitForSeconds(0.1f);
            e.GetRandomTarget();
            e.StartShooting();
        }
    }

    public int GetDiedEnemyRoam()
    {
        int numDie = 0;
        foreach (EnemyRoam_GS1 e in enemiesRoam)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }
    public int GetDiedEnemyRound2()
    {
        int numDie = 0;
        foreach (EnemyR2V3MoveRound2 e in enemiesR2V3)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }
    public void SetupEnemyR2Round2()
    {
        StartCoroutine(WaitSetupEnemyRound2(enemyRound21, 0));
        StartCoroutine(WaitSetupEnemyRound2(enemyRound22, 1.5f));
        StartCoroutine(WaitSetupEnemyRound2(enemyRound23, 3f));
        StartCoroutine(WaitSetupEnemyRound2(enemyRound24, 4.5f));
        StartCoroutine(WaitSetupEnemyRound2(enemyRound25, 6f));
    }
    private IEnumerator WaitSetupEnemyRound2(EnemyR2V3MoveRound2[] enemy, float time)
    {
        GameManager_GS1.Ins.ShowWarning(false);
        yield return new WaitForSeconds(time);

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].MoveToPos();
        }
    }


    #endregion

    #region Endcard
    public void SetupEnemy()
    {
        StartCoroutine(WaitSetupEnemy(enemy1, 0));
        StartCoroutine(WaitSetupEnemy(enemy2, 0.5f));
        StartCoroutine(WaitSetupEnemy(enemy3, 1f));
    }
    private IEnumerator WaitSetupEnemy(EnemyEndcard[] enemy, float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].MoveToPos();
        }
    }
    #endregion

    //   private void WaitToShowBoss(float time)
    //   {
    //       Invoke(nameof(ShowBoss), time);
    //   }

    //   private void ShowBoss()
    //{
    //       bossEndCard.DOLocalMoveY(5, 1.5f).SetEase(Ease.Linear);
    //}
}
