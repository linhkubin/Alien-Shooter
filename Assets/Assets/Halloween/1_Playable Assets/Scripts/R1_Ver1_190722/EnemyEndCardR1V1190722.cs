using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyEndCardR1V1190722 : MonoBehaviour
{
    public Transform pos;
    public float time_Move_to_Player;
    public float time_Move_to_Top = 0.5f;
    public Transform tf_ThisEnemy;

    private Animator anim;

    public float health;

    public bool canTakeDame = false;

    public bool hasFinishMove = false;
    Vector3 _posTop_L = new Vector3(-2, 2, 0);
    Vector3 _posTop_R = new Vector3(2, 2, 0);

    private void Start()
    {
        time_Move_to_Player = 1;
        tf_ThisEnemy = transform;
        anim = GetComponentInChildren<Animator>();

        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }


    public void Set_MoveToPos_To_Player(Vector3 _posPlayer)
    {
        Vector3 _posOri = tf_ThisEnemy.position;
        Vector3 _posTop =  Vector3.zero;
        if (Random.Range(0, 2) == 0)
        {
            _posTop = tf_ThisEnemy.position + new Vector3(_posTop_L.x, _posTop_L.y, _posTop_L.z);
        }
        else
        {
            _posTop = tf_ThisEnemy.position + new Vector3(_posTop_R.x, _posTop_R.y, _posTop_R.z);
        }
        transform.DOJump(_posPlayer,15,1, time_Move_to_Player).OnComplete(() => {
                transform.DOMove(_posOri, time_Move_to_Player);
        });
    }

    public void MoveToPos()
    {
        if (this != null && hasFinishMove == false)
        {
            transform.DOMove(pos.position, 1).OnComplete(() => {
                canTakeDame = true;

                hasFinishMove = true;
               
            });
        }
    }
    
    public virtual void DestroyShip()
    {
        if (EnemyManager_R1.Ins.GetDiedEnemyWave2() == 4)
        {
            EnemyManager_R1.Ins.UpdateHealth2(20);
        }

        if (!EnemyManager_R1.Ins.spawnedBooster_5 && EnemyManager_R1.Ins.GetDiedEnemyWave2() == 4)
        {
            Instantiate(EnemyManager_R1.Ins.bulletPrefabs, transform.position, Quaternion.identity);
            EnemyManager_R1.Ins.spawnedBooster_5 = true;
        }

        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    public void StartShooting()
    {
        if(this != null)
        {
            StartCoroutine(IE_Shooting());
        }
        
    }
    public void Shooting()
    {
        ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_BULLET_TAG, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(canTakeDame == true)
        {
            if (collision.CompareTag(Constant.BULLET_TAG))
            {
                TakeDamage(Constant.BULLET_DAMAGE);
            }

            if (collision.CompareTag(Constant.PLAYER_TAG))
            {
                TakeDamage(100);
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            if (EnemyManager_R1.Ins.list_enemiesRound2.Contains(this))
            {
                tf_ThisEnemy.DOKill();
                EnemyManager_R1.Ins.list_enemiesRound2.Remove(this);
            }
            EnemyManager_R1.Ins.PlayExplosionSound();
            DestroyShip();
        }
    }

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.8f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
