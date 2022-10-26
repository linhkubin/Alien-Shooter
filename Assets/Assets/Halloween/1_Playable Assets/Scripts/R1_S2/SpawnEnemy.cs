using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnEnemy : MonoBehaviour
{
    public static SpawnEnemy Ins;
    public bool canSpawn = true;
    private Vector3 target;

    public List<EnemyRoam_R1S2> listEnemy = new List<EnemyRoam_R1S2>();
    public Transform SpawnPoint;
    public int enemyCount;
    public int limitEnemy;
    public float timeCounter;
    public float maxTimeCounter;

    private Vector3 randomPos;

    public int totalEnemy;

    private GameObject PrefabParentEnemy;

    public Player_R1S2 player;
    private void Awake()
    {
        Ins = this;
    }
    void Update()
    {
        if(GameManager_R1S2.Ins.isLose == false)
        {
            randomPos = new Vector3(Random.Range(-5, 5), Random.Range(14, 18));

            if (totalEnemy <= 40 && StackShipLogic.Ins.listShipUp.Count > 0)
            {
                spawnEnemy();

                if (enemyCount >= 15)
                {
                    StartCoroutine(RespawnEnemy());
                }
            }

            if (totalEnemy <= 10)
            {
                maxTimeCounter = 1f;
            }
            else if (totalEnemy >= 30 && totalEnemy < 40)
            {
                maxTimeCounter = 0.8f;
            }
            else if (totalEnemy >= 40)
            {
                maxTimeCounter = 0.45f;
            }
        }

        if (StackShipLogic.Ins.listShipUp == null)
        {
            this.enabled = false;
        }


    }

    public void  spawnEnemy()
    {
        float xPos = Random.Range(-4.5f, 4.5f);

        int CountEnemyNumber = Random.Range(2, 5);

        float deltaX = 1f;

        if (canSpawn == true)
        {
            if (enemyCount == limitEnemy)
            {
                return;
            }

            timeCounter += Time.deltaTime;

            if (timeCounter >= maxTimeCounter)
            {
                timeCounter = 0;

                PrefabParentEnemy = ObjectPooling.Ins.SpawnFromPool(Constant.PARENT_ENEMY_TAG, new Vector3(xPos, 18), Quaternion.identity);

                for (int i = 0; i < CountEnemyNumber; i++)
                {
                    float posEnemy = xPos + i * deltaX;

                    Transform enemyTrans = ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_TAG, new Vector3(posEnemy, 18), Quaternion.identity).GetComponent<EnemyMove>().transform;

                    enemyTrans.SetParent(PrefabParentEnemy.transform);

                }
                
                enemyCount++;
            }
        }
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(1f);

        enemyCount = 0;
    }
}
