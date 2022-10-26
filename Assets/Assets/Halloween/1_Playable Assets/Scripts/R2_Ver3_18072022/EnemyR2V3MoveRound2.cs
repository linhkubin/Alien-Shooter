using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyR2V3MoveRound2 : MonoBehaviour
{
    public Animator anim;

    public float health;
    public Transform pos;
    public Transform PosCenter;
    public bool canTakeDamage = false;
    EnemyManager_GS1 enemyManager;
    private void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        enemyManager = EnemyManager_GS1.Ins;
    }

    private void Update()
    {
        if (transform.localPosition.y <= -39)
        {
            Destroy(this.gameObject);
        }
    }

    public void MoveToPos()
    {
        //transform.DOMove(PosCenter.position, 0.7f).OnComplete(() => {

        transform.DOMove(pos.position, 1.2f).OnComplete(() => {
            canTakeDamage = true;

            transform.DOMoveY(-100, 100f);

        });
        //});
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.BULLET_TAG) || other.CompareTag(Constant.BULLET_GUN_LEFT) || other.CompareTag(Constant.BULLET_GUN_RIGHT) || other.CompareTag(Constant.BULLET_SEMICIRCLE))
        {
            if (canTakeDamage == true)
            {
                TakeDamage(Constant.BULLET_DAMAGE);
            }
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
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.8f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
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
}
