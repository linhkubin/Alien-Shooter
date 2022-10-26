using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager_R1_1 : MonoBehaviour
{
    #region Singleton
    public static EnemyManager_R1_1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource explosion_Sound;

    public GameObject bulletPrefabs;

    public GameObject round1, round2;
    public GameObject round1Pos, round2Pos;

    private Player_R1_1 player;

    //=================== ROUND 1 ==================
    [HideInInspector]
    public int numGalaEnemy;
    public EnemyGala_V5[] triangleLeft;
    public EnemyGala_V5[] triangleRight;
    public EnemyGala_V5[] rectangle;
    public EnemyGala_V5[] line;
    private float speedInitRound1 = 1f;

    public Transform[] triangleLeftPos;
    public Transform[] triangleRightPos;
    public Transform[] rectanglePos;
    public Transform[] linePos;

    private float speedTriangleLeft = 1.5f;
    private float speedTriangleRight = 0.5f;
    private float speedRectangle = 1.5f;
    private float speedLine = 1f;

    private bool isRound1 = true;

    //=================== ROUND 2 ====================
    [HideInInspector]
    public int numEnemy;
    public Transform enemyContainer;
    public Enemy_R1_1[] enemies;
    public Enemy_R1_1[] leftEnemies;
    public Enemy_R1_1[] rightEnemies;
    public Transform leftPos, rightPos;

    public int enemies_num;
    public bool spawnedBooster_3 = false;
    public bool spawnedBooster_5 = false;
    public bool isRound2 = false;


    void Start()
    {
        player = Player_R1_1.Ins;

        numGalaEnemy = triangleLeft.Length + triangleRight.Length + rectangle.Length + line.Length;
        round2.SetActive(false);

        enemies_num = enemies.Length;

        SetupEnemyRound1();
    }

    private void FixedUpdate()
    {
        if (isRound1)
        {
            if (GetDiedGalaEnemy() == numGalaEnemy)
            {
                isRound1 = false;
                Destroy(round1);
                Destroy(round1Pos);

                //Setup Round2
                round2.SetActive(true);
                player.SwitchShip();

                isRound2 = true;
                numEnemy = enemies.Length;
                SetupEnemies(leftEnemies);
                SetupEnemies(rightEnemies);
            }
        }

        else if (isRound2)
        {
            if (GetDiedEnemy() == numEnemy)
            {
                isRound2 = false;
                Destroy(round2);
                Destroy(round2Pos);

                //Setup EndCard
                player.TurnOffBullet();
            }
        }
    }

    public void PlayExplosionSound()
    {
        explosion_Sound.PlayOneShot(explosion_Sound.clip);
    }

    #region ROUND_1
    private void SetupEnemyRound1()
    {
        for (int i = 0; i < triangleLeft.Length; i++)
        {
            triangleLeft[i].MoveToPos(triangleLeft[i].initPos, speedInitRound1);
            triangleLeft[i].curMove += i;
        }
        for (int i = 0; i < triangleRight.Length; i++)
        {
            triangleRight[i].MoveToPos(triangleRight[i].initPos, speedInitRound1);
            triangleRight[i].curMove += i;
        }
        for (int i = 0; i < rectangle.Length; i++)
        {
            rectangle[i].MoveToPos(rectangle[i].initPos, speedInitRound1);
            rectangle[i].curMove += i;
        }
        for (int i = 0; i < line.Length; i++)
        {
            line[i].MoveToPos(line[i].initPos, speedInitRound1);
            line[i].curMove += i;
        }

        Invoke(nameof(StartMoveEnemy), 1f);
    }

    private void StartMoveEnemy()
    {
        StartCoroutine(IE_MoveEnemyRound1(triangleLeft, triangleLeftPos, speedTriangleLeft, 1f, 1.5f));
        StartCoroutine(IE_MoveEnemyRound1(triangleRight, triangleRightPos, speedTriangleRight, 0.25f, 0.25f));
        StartCoroutine(IE_MoveEnemyRound1(rectangle, rectanglePos, speedRectangle, 0.3f, 1f));
        StartCoroutine(IE_MoveEnemyRound1(line, linePos, speedLine, 0.75f, 0.75f));
    }

    private void MoveEnemyRound1(EnemyGala_V5[] enemies, Transform[] positions, float speed)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].curMove += 1;
                int nextMove = enemies[i].curMove % enemies.Length;

                enemies[i].MoveToPos(positions[nextMove], speed);
            }
            else continue;
        }
    }

    IEnumerator IE_MoveEnemyRound1(EnemyGala_V5[] enemies, Transform[] positions, float speed, float time1, float time2)
    {
        yield return new WaitForSeconds(time1);
        MoveEnemyRound1(enemies, positions, speed);
        yield return new WaitForSeconds(time2);
        StartCoroutine(IE_MoveEnemyRound1(enemies, positions, speed, time1, time2));
    }

    public int GetDiedGalaEnemy()
    {
        int numDie = 0;
        for (int i = 0; i < triangleLeft.Length; i++)
        {
            if (triangleLeft[i] == null) numDie++;
        }
        for (int i = 0; i < triangleRight.Length; i++)
        {
            if (triangleRight[i] == null) numDie++;
        }
        for (int i = 0; i < rectangle.Length; i++)
        {
            if (rectangle[i] == null) numDie++;
        }
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == null) numDie++;
        }
        return numDie;
    }
    #endregion

    #region ROUND_2

    public int GetDiedEnemy()
    {
        int numDie = 0;
        foreach (Enemy_R1_1 e in enemies)
        {
            if (e == null) numDie++;
        }
        return numDie;
    }

    public void UpdateHealth(int health)
    {
        foreach (Enemy_R1_1 e in enemies)
        {
            if (e != null)
            {
                e.health = health;
            }
        }
    }

    public void StartShooting()
    {
        foreach (Enemy_R1_1 e in enemies)
        {
            e.StartShooting();
        }
    }

    private void SetupEnemies(Enemy_R1_1[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            StartCoroutine(IE_SetupEnemy(enemies[i], i / 10f));
        }

        StartCoroutine(IE_MoveEnemyContainer());
        StartShooting();
    }

    IEnumerator IE_SetupEnemy(Enemy_R1_1 e, float idx)
    {
        yield return new WaitForSeconds(0.25f + idx);
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
    #endregion
}
