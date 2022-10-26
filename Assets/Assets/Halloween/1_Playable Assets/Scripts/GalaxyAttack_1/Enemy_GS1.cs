using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GS1 : MonoBehaviour
{
    public float health;
    public Vector3[] path;
    public Transform initPos;
    public Animator anim;
    public int idx = 0;
    public bool isLeft;
    private Vector3 targetPos;
    [HideInInspector] public int curMove = 0;

    void Start()
    {
        targetPos = transform.position;
        anim.SetFloat("Speed", Random.Range(0.5f, 1.5f));
        transform.position = initPos.position;
    }

    
    public void MoveToPos()
    {
        path = new Vector3[3];
        path[0] = initPos.position;
        path[2] = targetPos;
        if (isLeft)
		{
            path[1] = EnemyManager_GS1.Ins.posLeft.position;
		}
        else
		{
            path[1] = EnemyManager_GS1.Ins.posRight.position;
        }
        //transform.DOMove(targetPos, 0.5f);
        transform.DOPath(path, 1.2f, PathType.CatmullRom).SetEase(Ease.Linear);

    }

    public void MoveAroundStar(Transform pos, float moveDuration)
    {
         transform.DOMove(pos.position, moveDuration).SetEase(Ease.Linear).OnComplete(() => {
                
                idx++;
        });

    }
         

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_GALA_TAG) || collision.CompareTag(Constant.BULLET_TAG) || collision.CompareTag(Constant.BULLET_GUN_LEFT) || collision.CompareTag(Constant.BULLET_GUN_RIGHT) || collision.CompareTag(Constant.BULLET_SEMICIRCLE))
        {
            TakeDamage(Constant.BULLET_DAMAGE);

            if (EnemyManager_GS1.Ins.GetDiedEnemy() >= 1 && !EnemyManager_GS1.Ins.spawnedShip)
			{
                EnemyManager_GS1.Ins.spawnedShip = true;
                EnemyManager_GS1.Ins.SpawnShipBoost(transform);
            }

            if (EnemyManager_GS1.Ins.GetDiedEnemy() >= 4 && !EnemyManager_GS1.Ins.spawnedBooster)
            {
                EnemyManager_GS1.Ins.spawnedBooster = true;
                EnemyManager_GS1.Ins.SpawnBooster(transform);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_GS1.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
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
        float randWait = Random.Range(5f, 10f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.75f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
