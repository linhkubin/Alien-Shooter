using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Enemy_EndCard_V4 : MonoBehaviour
{   
    public Transform initPos;

    void Start()
	{
        StartCoroutine(IE_Shooting());
    }

    public void MoveToPos()
    {
        transform.DOMove(initPos.position, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.PLAYER_TAG))
        {
            EnemyManager_V4.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator IE_Shooting()
    {
        float randWait = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.75f)
        {
            Instantiate(EnemyManager_V4.Ins.bulletEnemyPrefabs, transform.position, Quaternion.identity);
        }

        StartCoroutine(IE_Shooting());
    }
}
