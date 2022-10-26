using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEndcard_GSP1 : MonoBehaviour
{
    public Animator anim;

    public Transform targetPos;

    void Start()
    {
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }

    public void MoveToPos()
    {
        Vector3[] vectorPath = new Vector3[EndCard_GSP1.Ins.movePath.Length + 1];
        for (int i = 0; i < vectorPath.Length - 1; i++)
        {
            vectorPath[i] = EndCard_GSP1.Ins.movePath[i].position;
        }
        vectorPath[vectorPath.Length - 1] = targetPos.position;

        transform.DOPath(vectorPath, 1.5f, PathType.CatmullRom).SetEase(Ease.Linear);
    }
}
