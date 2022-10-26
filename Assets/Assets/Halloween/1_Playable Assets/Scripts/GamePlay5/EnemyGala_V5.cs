using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGala_V5 : Spaceship_V5
{
    public Transform initPos;
    public Transform sprite;

    public int idx = 0;

    [HideInInspector] public int curMove = 0;

    private float endValue = 0.25f, time = 0.3f;

    private EnemyManager_R1_Remake_2 enemyManager;

    private void Start()
    {
        enemyManager = EnemyManager_R1_Remake_2.Ins;
    }


    public override void MoveToPos(Transform pos, float moveDuration)
	{
        transform.DOMove(pos.position, moveDuration).SetEase(Ease.Linear).OnComplete(()=> {
            //Di chuyen ruoi len xuong
            if (idx % 2 == 0)
            {
                sprite.DOLocalMoveY(endValue, time).OnComplete(() =>
                {
                    sprite.DOLocalMoveY(-endValue, time).OnComplete(() =>
                    {
                        sprite.DOLocalMoveY(0, time);
                    }); ;
                });
            }
            else
            {
                sprite.DOLocalMoveY(-endValue, time).OnComplete(() =>
                {
                    sprite.DOLocalMoveY(endValue, time).OnComplete(() =>
                    {
                        sprite.DOLocalMoveY(0, time);
                    }); ;
                });
            }
            idx++;
        });
    }

	public override void DestroyShip()
    {
        if (!enemyManager.spawnedBooster_3 && enemyManager.GetDiedEnemyRound1() == 1)
        {
            Instantiate(enemyManager.bulletPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_3 = true;
        }
        else if (!enemyManager.spawnedBooster_5 && enemyManager.GetDiedEnemyRound1() == 6)
        {
            Instantiate(enemyManager.bulletPrefabs, transform.position, Quaternion.identity);
            enemyManager.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_GALAGA_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void StartShooting()
    {
        StartCoroutine(IE_Shooting());
    }

    public void Shooting()
    {
        ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_BULLET_TAG, transform.position, Quaternion.identity);
    }

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.9f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }

   
}
