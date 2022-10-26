using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_V5 : Spaceship_V5
{
    public Transform initPos;
    public Transform sprite;

    private float endValue = 0.1f, time = 0.25f;

	private void Start()
	{
        StartCoroutine(IE_MoveEnemy());
    }

	public override void DestroyShip()
    {
        if (!EnemyManager_V5.Ins.spawnedBooster_3 && EnemyManager_V5.Ins.GetDiedEnemy() == 1)
        {
            Instantiate(EnemyManager_V5.Ins.bulletPrefabs, transform.position, Quaternion.identity);
            EnemyManager_V5.Ins.spawnedBooster_3 = true;
        }
        else if (!EnemyManager_V5.Ins.spawnedBooster_5 && EnemyManager_V5.Ins.GetDiedEnemy() == 5)
        {
            Instantiate(EnemyManager_V5.Ins.boosterPrefabs, transform.position, Quaternion.identity);
            EnemyManager_V5.Ins.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator IE_MoveEnemy()
    {
        float rand = Random.Range(0, 1f);
        yield return new WaitForSeconds(rand);
        
        if (rand> 0.5f)
        {
            sprite.DOLocalMoveY(endValue, time).OnComplete(() => {
                sprite.DOLocalMoveY(-endValue, time).OnComplete(() => {
                    sprite.DOLocalMoveY(0, time);
                }); ;
            });
        }
        else
        {
            sprite.DOLocalMoveY(-endValue, time).OnComplete(() => {
                sprite.DOLocalMoveY(endValue, time).OnComplete(() => {
                    sprite.DOLocalMoveY(0, time);
                }); ;
            });
        }

        yield return new WaitForSeconds(Random.Range(0, 1f));
        StartCoroutine(IE_MoveEnemy());
    }
}
