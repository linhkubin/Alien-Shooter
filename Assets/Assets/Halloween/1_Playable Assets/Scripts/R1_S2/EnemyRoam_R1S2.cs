using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoam_R1S2 : MonoBehaviour
{
    public float health;
    public Animator anim;

    private Vector3 target;

    private bool isFirst = true;

    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.5f, 1.5f));
    }

    public void GetRandomTarget()
    {
        target = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 8f), 0);
        transform.DOMove(target, Random.Range(2f, 5f)).OnComplete(() =>
        {
            GetRandomTarget();
        });
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_R1S2.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
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