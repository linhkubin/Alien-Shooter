using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_R1 : MonoBehaviour
{
    public Animator anim;

    public float health;
    public Transform pos;
    public bool isLeft;
    public bool canMove = false;

    //public Vector3[] path;
    public Vector3 startPos;

    private float speed = 20f;
    private float moveDuration = 1f;
    private bool reachedStartPos = false;

    void Start()
    {
        //if (isLeft) startPos = EnemyManager_R1.Ins.leftPos.position;
        //else startPos = EnemyManager_R1.Ins.rightPos.position;

       
    }

    //void Update()
    //{
        //if (canMove)
        //{
        //    if (!reachedStartPos)
        //    {
        //        float distance = Vector3.Distance(transform.position, startPos);
        //        if (distance > 0.25f)
        //        {
        //            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

        //        }
        //        else
        //        {
        //            reachedStartPos = true;
        //            MoveToPos();
        //        }
        //    }
        //}
    //}

    //public void MoveToPos()
    //{
    //    if (isLeft)
    //    {
    //        startPos = Enemy_R1_Remake_V1.Ins.rightPos.position;
    //    }
    //    else
    //    {
    //        startPos = Enemy_R1_Remake_V1.Ins.leftPos.position;
    //    }

    //    Vector3[] path = {startPos, pos.position};

    //    transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
    //    //transform.DOMove(pos.position, moveDuration);
    //}
    public void MoveToPos()
    {
        if (isLeft)
        {
            startPos = EnemyManager_R1.Ins.rightPos.position;
        }
        else
        {
            startPos = EnemyManager_R1.Ins.leftPos.position;
        }

        Vector3[] path = {startPos, pos.position};

        transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
        //transform.DOMove(pos.position, moveDuration);
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }

        if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            TakeDamage(5);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            //Enemy_R1_Remake_V1.Ins.PlayExplosionSound();
            EnemyManager_R1.Ins.PlayExplosionSound();
            DestroyShip();
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

  //  public virtual void DestroyShip()
  //  {
  //      if (Enemy_R1_Remake_V1.Ins.GetDiedEnemy() == 2)
  //      {
  //          Enemy_R1_Remake_V1.Ins.UpdateHealth(2);
  //      }

  //      if (!Enemy_R1_Remake_V1.Ins.spawnedBooster_3 && Enemy_R1_Remake_V1.Ins.GetDiedEnemy() == 1)
		//{
		//	Instantiate(Enemy_R1_Remake_V1.Ins.bulletPrefabs, transform.position, Quaternion.identity);
  //          Enemy_R1_Remake_V1.Ins.spawnedBooster_3 = true;
		//}
  //      else if (!Enemy_R1_Remake_V1.Ins.spawnedBooster_5 && Enemy_R1_Remake_V1.Ins.GetDiedEnemy() == 6)
  //      {
  //          Instantiate(Enemy_R1_Remake_V1.Ins.boosterPrefabs, transform.position, Quaternion.identity);
  //          Enemy_R1_Remake_V1.Ins.spawnedBooster_5 = true;
  //      }

  //      ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
  //      Destroy(gameObject);
  //  }
    public virtual void DestroyShip()
    {
        if (EnemyManager_R1.Ins.GetDiedEnemy() == 2)
        {
            EnemyManager_R1.Ins.UpdateHealth(2);
        }

        if (!EnemyManager_R1.Ins.spawnedBooster_3 && EnemyManager_R1.Ins.GetDiedEnemy() == 1)
		{
			Instantiate(EnemyManager_R1.Ins.bulletPrefabs, transform.position, Quaternion.identity);
            EnemyManager_R1.Ins.spawnedBooster_3 = true;
		}
        else if (!EnemyManager_R1.Ins.spawnedBooster_5 && EnemyManager_R1.Ins.GetDiedEnemy() == 6)
        {
            Instantiate(EnemyManager_R1.Ins.boosterPrefabs, transform.position, Quaternion.identity);
            EnemyManager_R1.Ins.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(3f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.7f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
