using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_R1S2 : MonoBehaviour
{
    public float health;
    public Vector3[] path;
    public Transform initPos;
    public Animator anim;

    private Vector3 targetPos;

    void Awake()
    {
        targetPos = transform.position;
        anim.SetFloat("Speed", Random.Range(0.5f, 1.5f));
        transform.position = initPos.position;
    }

    //public void MoveToPos()
    //{
    //    path = new Vector3[12];
    //    path[0] = initPos.position;
    //    path[11] = targetPos;
    //    for(int i = 0; i < EnemyManager_R1_Remake_2.Ins.path.Length; i++)
    //    {
    //        path[i + 1] = EnemyManager_R1_Remake_2.Ins.path[i].position;
    //    }
        
    //    transform.DOPath(path, 1.8f, PathType.CatmullRom)/*.SetEase(Ease.Linear)*/;
    //}
    public void MoveToPos()
    {
        path = new Vector3[12];
        path[0] = initPos.position;
        path[11] = targetPos;
        for(int i = 0; i < EnemyManager_R1_Remake_2.Ins.path.Length; i++)
        {
            path[i + 1] = EnemyManager_R1_Remake_2.Ins.path[i].position;
        }
        
        transform.DOPath(path, 1.8f, PathType.CatmullRom)/*.SetEase(Ease.Linear)*/;
    }

    //public void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.CompareTag(Constant.BULLET_GALA_TAG) || collision.CompareTag(Constant.BULLET_TAG))
    //    {
    //        TakeDamage(Constant.BULLET_DAMAGE);

    //        if (EnemyManager_R1.Ins.GetDiedEnemy() >= 4 && !EnemyManager_R1S2.Ins.spawnedBooster)
    //        {
    //            EnemyManager_R1S2.Ins.spawnedBooster = true;
    //            EnemyManager_R1S2.Ins.SpawnBooster(transform);
    //        }
    //    }
    //}

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;

    //    if (health < 0)
    //    {
    //        EnemyManager_R1_Remake_2.Ins.PlayExplosionSound();
    //        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
    //        Destroy(gameObject);
    //    }
    //}
    
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_R1_Remake_2.Ins.PlayExplosionSound();
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
