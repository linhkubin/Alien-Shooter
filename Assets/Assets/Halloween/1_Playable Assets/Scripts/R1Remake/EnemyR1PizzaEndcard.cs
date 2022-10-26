using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyR1PizzaEndcard : MonoBehaviour
{
    public Animator anim;

    public float health;
    public Transform pos;

    private EnemyManagerR1Pizza enemyManager;
    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        enemyManager = EnemyManagerR1Pizza.Ins;
    }

    public void MoveToPos()
    {
        transform.DOMove(pos.position, 1);
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
}
