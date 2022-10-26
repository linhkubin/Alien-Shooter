using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyR1Pizza : MonoBehaviour
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
            path[1] = EnemyManagerR1Pizza.Ins.posLeft.position;
        }
        else
        {
            path[1] = EnemyManagerR1Pizza.Ins.posRight.position;
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

            if (EnemyManagerR1Pizza.Ins.GetDiedEnemy() >= 1 && !EnemyManagerR1Pizza.Ins.spawnedShip)
            {
                EnemyManagerR1Pizza.Ins.spawnedShip = true;
                EnemyManagerR1Pizza.Ins.SpawnShipBoost(transform);
            }

            if (EnemyManagerR1Pizza.Ins.GetDiedEnemy() >= 8 && !EnemyManagerR1Pizza.Ins.spawnedBooster)
            {
                EnemyManagerR1Pizza.Ins.spawnedBooster = true;
                EnemyManagerR1Pizza.Ins.SpawnBooster(transform);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManagerR1Pizza.Ins.PlayExplosionSound();
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
