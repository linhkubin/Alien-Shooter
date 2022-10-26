using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EnemyEndCard_R1_1 : MonoBehaviour
{
    public Transform initPos;
    public int curMove;

    public void MoveToPos(Transform pos)
    {
        transform.DOMove(pos.position, 0.5f);
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
        float randWait = Random.Range(1f, 5f);
        yield return new WaitForSeconds(randWait);

        float rand = Random.Range(0, 1f);
        if (rand > 0.9f)
        {
            Shooting();
        }

        StartCoroutine(IE_Shooting());
    }
}
