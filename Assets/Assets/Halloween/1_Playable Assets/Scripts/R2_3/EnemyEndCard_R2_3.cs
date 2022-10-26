using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndCard_R2_3 : MonoBehaviour
{
    public Animator anim;

    public Transform pos;
    public Vector3 startPos;

    public bool isLeft;

    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }

    public void MoveToPos()
    {
        if (isLeft)
        {
            startPos = EndCard_R2_3.Ins.rightPos.position;
        }
        else
        {
            startPos = EndCard_R2_3.Ins.leftPos.position;
        }

        Vector3[] path = { startPos, pos.position };

        transform.DOPath(path, 1f, PathType.CatmullRom).SetEase(Ease.Linear);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            EnemyManager_R2_3.Ins.PlayExplosionSound();

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
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.9f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
