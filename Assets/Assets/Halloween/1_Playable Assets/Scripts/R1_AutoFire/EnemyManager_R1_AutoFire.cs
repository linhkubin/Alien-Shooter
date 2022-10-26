using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_R1_AutoFire : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_R1_AutoFire Ins;
    private void Awake()
    {
        Ins = this;
    }
    #endregion

    public Player_R1 player;
    private Transform tf_Player;
    public EnemyAutoFire[] enemies;
    public AudioSource explosion_Sound;
    public int enemies_num;

    [HideInInspector]
    public bool spawnedBooster_3 = false;
    [HideInInspector]
    public bool spawnedBooster_5 = false;

    public GameObject bulletEnemyPrefabs;
    public GameObject boosterPrefabs;
    public GameObject bulletPrefabs;
    public bool canSetupEnemy = false;

    public float speedMove;
    [HideInInspector]
    public int countCoin;
    public AudioSource coinCollect;

    void Start()
    {
        speedMove = 2f;
        enemies_num = enemies.Length;
        tf_Player = player.gameObject.transform;
    }

    void Update()
    {
        if(canSetupEnemy == true && GameManager_R1.Ins.isWin == false)
        {
            SetupEnemy();
        }
    }

    public void SetupEnemy()
    {
        foreach(EnemyAutoFire e in enemies)
        {
            if(e != null)
            {
                e.EnemyMove(tf_Player);
                e.autoDestroy(tf_Player);
            }
        }
    }

    public void UpdateHealth(int health)
    {
        foreach (EnemyAutoFire e in enemies)
        {
            if (e != null)
            {
                e.health = health;
            }
        }
    }

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (EnemyAutoFire e in enemies)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }
}
