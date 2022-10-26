using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_R1 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_R1 Ins;
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
    public EnemyEndCardR1V1190722[] enemiesRound2;
    public List<EnemyEndCardR1V1190722> list_enemiesRound2;
    public int count_Random;
    public int count_Con_Lai_Tren_Man_Hinh;




    [HideInInspector]
    public int enemies_num;
    public int enemiesRound2_num;

    public Enemy_R1[] leftEnemies, rightEnemies;

    [HideInInspector]
    public bool spawnedBooster_3 = false;
    [HideInInspector]
    public bool spawnedBooster_5 = false;
   
    public bool isFist_Call = false;
    public bool isFist_Call_2 = false;
    public bool hasWarning = false;
    public bool hasWarning2 = false;

    public Enemy_R1S2[] enemiesEndcard;

    public Transform[] path;

    public bool isSetupEndcard = false;
    void Start()
    {
        for (int i = 0; i < enemiesRound2.Length; i++)
        {
            list_enemiesRound2.Add(enemiesRound2[i]);
        }


        count_Random = 10;
        count_Con_Lai_Tren_Man_Hinh = enemiesRound2.Length;
         enemies_num = enemies.Length;
        enemiesRound2_num = enemiesRound2.Length;


        //EnemyStart();
        SetupEnemies(leftEnemies);
        SetupEnemies(rightEnemies);

        //StartCoroutine(IE_MoveEnemyContainer());
    }
    private void Update()
    {
        if (GetDiedEnemy() == enemies_num)
        {
            if (hasWarning == false)
            {
                hasWarning = true;
                GameManager_R1.Ins.ShowWarning(true);
            }

            Invoke(nameof(setUpEnemyEndcard), 1f);
            //setUpEnemyEndcard();
            isFist_Call = true;
        }
        if (isFist_Call)
        {
            if (!isFist_Call_2)
            {
                isFist_Call_2 = true;
                StartCoroutine(IE_Set_Downt());
            }
        }
        if (GetDiedEnemyWave2() == enemiesRound2_num)
        {
            Player_R1.Ins.TurnOffBullet();

            if(isSetupEndcard == false)
            {
                isSetupEndcard = true;
                Invoke(nameof(SetupEnemyRound2), 1.5f);
                
            }
        }
    }

    public void SetupEnemyRound2()
    {
        foreach (Enemy_R1S2 e in enemiesEndcard)
        {
            e.MoveToPos();
        }
    }

    public void setUpEnemyEndcard()
    {
        if(hasWarning2 == false)
        {
            hasWarning2 = true;
            GameManager_R1.Ins.ShowWarning(false);
        }
        
        foreach (EnemyEndCardR1V1190722 e in enemiesRound2)
        {
            e.MoveToPos();
        }
        
    }

    IEnumerator IE_Set_Downt()
    {

        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
        yield return new WaitForSeconds(2);
        EnemyManager_R1.Ins.Set_Move_Random_Enemy();
    }

    public void Set_Move_Random_Enemy()
    {
        List<int> _list_Index_Random = new List<int>();
        //enemiesRound2.Length
        if (list_enemiesRound2.Count > 0)
        {
            for (int i = 0; i < count_Random; i++)
            {
                int _ii = Random.Range(0, list_enemiesRound2.Count);
                if (!_list_Index_Random.Contains(_ii))
                {
                    Vector3 _vec = new Vector3(Player_R1.Ins.tf_Player.position.x, Player_R1.Ins.tf_Player.position.y, Player_R1.Ins.tf_Player.position.z);
                    list_enemiesRound2[_ii].Set_MoveToPos_To_Player(_vec);
                }
                _list_Index_Random.Add(_ii);
            }
        }
        

    }

    //

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
    public void UpdateHealth2(int health)
	{
        foreach (EnemyEndCardR1V1190722 e in enemiesRound2)
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
    public int GetDiedEnemyWave2()
    {
        int numDie = 0;
        foreach (EnemyEndCardR1V1190722 e in enemiesRound2)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    private void SetupEnemies(Enemy_R1[] enemies)
	{
        for (int i = 0; i < enemies.Length; i++) {
            StartCoroutine(IE_SetupEnemy(enemies[i], i/10f));
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
