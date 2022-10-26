using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRoamR1Pizza : MonoBehaviour
{
    public float health;
    public Animator anim;

    private Vector3 target;

    private bool isFirst = true;

    public bool canTakeDamage = false;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.5f, 1.5f));
    }

    public void GetRandomTarget()
    {
        target = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 8f), 0);
        transform.DOMove(target, Random.Range(3f, 4f)).OnComplete(() =>
        {
            GetRandomTarget();
        });
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG) || collision.CompareTag(Constant.BULLET_GUN_LEFT) || collision.CompareTag(Constant.BULLET_GUN_RIGHT))
        {
            if (canTakeDamage == true)
            {
                TakeDamage(Constant.BULLET_DAMAGE);
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
        float randWait = Random.Range(3f, 6f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.75f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
