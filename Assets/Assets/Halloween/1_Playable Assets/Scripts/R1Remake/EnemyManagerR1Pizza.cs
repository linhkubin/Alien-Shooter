using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Luna.Unity;

public class EnemyManagerR1Pizza : MonoBehaviour
{
    #region Singleton
    public static EnemyManagerR1Pizza Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion
    public AudioSource explosion_Sound;

    public GameObject boosterPrefabs;
    public GameObject shipPrefabs;
    //=================== ROUND 1 ==================
    [HideInInspector]
    public int numEnemy;
    public int numGalaEnemy;
    public Transform posLeft, posRight;
    public EnemyR1Pizza[] enemies;
    public bool spawnedShip = false;
    public bool spawnedBooster = false;



    public EnemyR1Pizza[] Pizza;
    public GameObject[] rectangle;
    public Transform[] pizzaPos;
    
    private float speedPizza = 1.5f;
    [SerializeField]
    private bool isRound1 = true;
    private bool isRound2 = false;
    //=================== ROUND 2 ====================
    [HideInInspector]
    public int numEnemy3;
    public EnemyRoamR1Pizza[] enemiesRoam;
    [SerializeField]
    private bool isRound3 = false;

    //=================== ENDCARD ====================

    public EnemyR1PizzaEndcard[] enemy1;
    public EnemyR1PizzaEndcard[] enemy2;
    public EnemyR1PizzaEndcard[] enemy3;

    public EnemyEndCardR1V1190722[] enemiesRound2;

    public GameObject boss;
    void Start()
    {
        numGalaEnemy = rectangle.Length + 1;
        numEnemy = enemies.Length;
        numEnemy3 = enemiesRoam.Length;
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
                Invoke(nameof(startEnemyRoam), 1.5f);
                //Invoke(nameof(SetupEnemyR2Round2), 1.5f);
                Invoke(nameof(DelayTakeDame), 4.15f);

                isRound3 = true;
                Analytics.LogEvent(Analytics.EventType.LevelWon, 1);
            }
        }
        else if (isRound3)
        {
            if (GetDiedEnemyRoam() == numEnemy3)
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

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    public void SpawnShipBoost(Transform pos)
    {
        Instantiate(shipPrefabs, pos.position, Quaternion.identity);
    }

    public void SpawnBooster(Transform pos)
    {
        Instantiate(boosterPrefabs, pos.position, Quaternion.identity);
    }

    public int GetDiedGalaEnemy()
    {
        int numDie = 0;
        for (int i = 0; i < rectangle.Length; i++)
        {
            if (rectangle[i] == null)
            {
                numDie++;
            }
        }
        if (boss == null) numDie++;
        return numDie;
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

    private void SetupEnemyRound2()
    {
        foreach (EnemyR1Pizza e in enemies)
        {
            e.MoveToPos();
            e.StartShooting();
        }

        for (int i = 0; i < Pizza.Length; i++)
        {
            Pizza[i].curMove += i;
        }
        Invoke(nameof(StartMoveEnemyStar), 1f);
    }

    private void StartMoveEnemyStar()
    {
        StartCoroutine(IE_MoveEnemyStar(Pizza, pizzaPos, speedPizza, 0.5f, 1.3f));
    }
    private void MoveEnemyStar(EnemyR1Pizza[] enemies, Transform[] positions, float speed)
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

    IEnumerator IE_MoveEnemyStar(EnemyR1Pizza[] enemies, Transform[] positions, float speed, float time1, float time2)
    {
        yield return new WaitForSeconds(time1);
        MoveEnemyStar(enemies, positions, speed);
        yield return new WaitForSeconds(time2);
        StartCoroutine(IE_MoveEnemyStar(enemies, positions, speed, time1, time2));
    }

    public void SetupEnemy()
    {
        StartCoroutine(WaitSetupEnemy(enemy1, 0));
        StartCoroutine(WaitSetupEnemy(enemy2, 0.5f));
        StartCoroutine(WaitSetupEnemy(enemy3, 1f));
    }
    private IEnumerator WaitSetupEnemy(EnemyR1PizzaEndcard[] enemy, float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].MoveToPos();
        }
    }

    public void startEnemyRoam()
    {
        if(this.gameObject.activeInHierarchy == true)
        {
            StartCoroutine(IE_SetupEnemyRound3());
        }
        else
        {
            return;
        }

    }
    IEnumerator IE_SetupEnemyRound3()
    {
        GameManager_GS1.Ins.ShowWarning(false);
        foreach (EnemyRoamR1Pizza e in enemiesRoam)
        {
            yield return new WaitForSeconds(0.1f);
            e.GetRandomTarget();
            e.StartShooting();
        }
    }

    public int GetDiedEnemyRoam()
    {
        int numDie = 0;
        foreach (EnemyRoamR1Pizza e in enemiesRoam)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    public void DelayTakeDame()
    {
        foreach (EnemyRoamR1Pizza e in enemiesRoam)
        {
            e.canTakeDamage = true;
        }
    }
}
