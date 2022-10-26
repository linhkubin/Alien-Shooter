using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float health;
    public Animator anim;
    private Transform thisPos;

    private float speed;

    public bool canMoving = false;

    public Transform ShipToDestroy;

    public bool hasCheckPlayer = false;

    public bool isUpdateHealth = false;
    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.5f, 1.5f));

        thisPos = this.transform;

        speed = Random.Range(6, 8);

        if(StackShipLogic.Ins.listShipUp.Count >= 1)
        {
            ShipToDestroy = StackShipLogic.Ins.listShipUp[Random.Range(Random.Range(0, StackShipLogic.Ins.listShipUp.Count), StackShipLogic.Ins.listShipUp.Count)].transform;
        }
        
    }

    private void Update()
    {
        if (canMoving)
        {
            EnemyMoving();
        }
        
        if(StackShipLogic.Ins.listShipUp.Count <= 0)
        {
            Destroy(this.gameObject);
        }

        if(GameManager_R1S2.Ins.isLose == true)
        {
            this.gameObject.SetActive(false);
        }

        if(StackShipLogic.Ins.listShipUp.Count >= 13 && isUpdateHealth == false)
        {
            isUpdateHealth = true;
            health = Random.Range(10, 15);
        }

    }

    public void EnemyMoving()
    {
        if (ShipToDestroy.GetComponent<StackShip>().isDead == false)
        {
            hasCheckPlayer = false;

            transform.position = Vector3.MoveTowards(thisPos.position, ShipToDestroy.position, speed * Time.deltaTime);

            if (Vector3.Distance(thisPos.position, ShipToDestroy.position) < 20f)
            {
                speed = 11;
            }
            if (Vector3.Distance(thisPos.position, ShipToDestroy.position) <= 0f)
            {
                TakeDamage(999);
                
            }
        }

        if (ShipToDestroy.GetComponent<StackShip>().isDead == true && StackShipLogic.Ins.listShipUp.Count >= 1 && hasCheckPlayer == false)
        {
            hasCheckPlayer = true;
            ShipToDestroy = StackShipLogic.Ins.listShipUp[Random.Range(Random.Range(0, StackShipLogic.Ins.listShipUp.Count), StackShipLogic.Ins.listShipUp.Count)].transform;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
            EnemyManager_R1S2.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);

            ObjectPooling.Ins.DespawnObject(this.tag, this.gameObject);

            SpawnEnemy.Ins.totalEnemy++;
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            if(transform.position.y <= 5.5)
            {
                TakeDamage(Constant.BULLET_DAMAGE);
            }
        }
        else if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            TakeDamage(999);
        }
    }
    //public void OnTriggerStay(Collider collision)
    //{
    //    if (collision.CompareTag(Constant.BULLET_TAG))
    //    {
    //        if(transform.position.y <= 5.5)
    //        {
    //            TakeDamage(Constant.BULLET_DAMAGE);
    //        }
    //    }
    //    else if (collision.CompareTag(Constant.PLAYER_TAG))
    //    {
    //        TakeDamage(Constant.BULLET_DAMAGE);
    //    }
    //}


}
