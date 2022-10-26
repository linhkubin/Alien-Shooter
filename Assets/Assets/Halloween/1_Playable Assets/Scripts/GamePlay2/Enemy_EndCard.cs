using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EndCard : MonoBehaviour
{
    public Transform pos;
    public bool isLeft;
    public bool canMove = false;

    private Vector3 startPos;

    private float speed = 20f;
    private float moveDuration = 1f;
    private bool reachedStartPos = false;
    void Start()
    {
        if (isLeft) startPos = EndCard_V2.Ins.leftPos.position;
        else startPos = EndCard_V2.Ins.rightPos.position;
    }

    void Update()
    {
        if (canMove)
        {
            if (!reachedStartPos)
            {
                float distance = Vector3.Distance(transform.position, startPos);
                if (distance > 0.25f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

                }
                else
                {
                    reachedStartPos = true;
                    MoveToPos();
                }
            }
        }
    }

    void MoveToPos()
    {
        transform.DOMove(pos.position, moveDuration);
    }
}
