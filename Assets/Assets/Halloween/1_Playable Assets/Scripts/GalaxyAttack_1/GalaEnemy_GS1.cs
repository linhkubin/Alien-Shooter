using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaEnemy_GS1 : MonoBehaviour
{
    public float health;

    public Transform targerPos;
    public Animator anim;
    public Transform sprite;

    [HideInInspector] public int curMove = 0;

    public int idx = 0;
    private float endValue = 0.1f, time = 0.25f; 

    void Start()
    {
        if (anim != null) anim.SetFloat("Speed", Random.Range(0.1f, 0.15f));
    }

    public void MoveToInitPos()
    {
        transform.DOMove(targerPos.position, 0.5f);
    }

    public void MoveToPos(Transform pos, float moveDuration)
    {
        transform.DOMove(pos.position, moveDuration).SetEase(Ease.Linear).OnComplete(() => {
            if (idx % 2 == 0)
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
            idx++;
        });
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_GALA_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_GS1.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_GALAGA_TAG, transform.position, Quaternion.identity);
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
