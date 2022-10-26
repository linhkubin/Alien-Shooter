using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_R1S2 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_R1S2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject boosterPrefabs;

    //=================== ROUND 2 ====================
    [HideInInspector]
    public int numEnemy;
    public Enemy_R1S2[] enemies;

    public Transform[] path;

    public bool spawnedBooster = false;

    [SerializeField]
    private bool isRound2 = true;

    //=================== ROUND 3 ====================
    [HideInInspector]
    public int numEnemy3;
    public EnemyRoam_R1S2[] enemiesRoam;
    [SerializeField]
    private bool isRound3 = false;

    //=================== ENDCARD ====================
    public Transform bossEndCard;

    void Start()
    {
        numEnemy = enemies.Length;
        numEnemy3 = enemiesRoam.Length;
        //SetupEnemyRound2();
    }

    private void FixedUpdate()
    {
        if (isRound2)
        {
            if (SpawnEnemy.Ins.totalEnemy >= 40)
            {
                isRound2 = false;
                GameManager_R1S2.Ins.ShowWarning(true);
                Invoke(nameof(SetupEnemyRound3), 2f);
                Boss_R1S2.Ins.WaitToShowBoss(3f);
                SpawnEnemy.Ins.canSpawn = false;



                isRound3 = true;
            }
        }
        else if (isRound3)
        {
            if (GetDiedEnemyRoam() == numEnemy3)
            {
                // Setup EndCard
                //Player_R1S2.Ins.WaitToTurnOffBullet(0.5f);
                //WaitToShowBoss(0.25f);

                //if (SpawnEnemy.Ins.totalEnemy >= 65)
                //{
                //    GameManager_R1S2.Ins.TurnOffBullet = true;
                //    GameManager_R1S2.Ins.canMoving = false;
                //}

                isRound3 = false;
            }
        }
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }


    #region ROUND_2
    public void SetupEnemyRound2()
    {
        foreach (Enemy_R1S2 e in enemies)
        {
            e.MoveToPos();
            //e.StartShooting();
        }
    }

    public void SpawnBooster(Transform pos)
    {
        Instantiate(boosterPrefabs, pos.position, Quaternion.identity);
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (Enemy_R1S2 e in enemies)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }
    #endregion

    public void SetupEnemyRound3()
    {
        StartCoroutine(IE_SetupEnemyRound3());
    }

    #region ROUND_3
    IEnumerator  IE_SetupEnemyRound3()
    {
        GameManager_R1S2.Ins.ShowWarning(false);
        foreach (EnemyRoam_R1S2 e in enemiesRoam)
        {
            yield return new WaitForSeconds(0f);
            e.GetRandomTarget();
            e.StartShooting();
        }
    }

    public int GetDiedEnemyRoam()
    {
        int numDie = 0;
        foreach (EnemyRoam_R1S2 e in enemiesRoam)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }
    #endregion

    private void WaitToShowBoss(float time)
    {
        Invoke(nameof(ShowBoss), time);
    }

    private void ShowBoss()
    {
        //bossEndCard.DOLocalMoveY(1, 1.5f).SetEase(Ease.Linear);
    }
}
