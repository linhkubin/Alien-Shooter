using UnityEngine;
using DG.Tweening;

public class EnemyRed : SpaceShip
{
    public Transform pos;
    public bool isLeft;

    private Vector3 startPos;

    private float speed = 4f;
    private float moveDuration = 1f;
    private bool reachedStartPos = false;

    void Start()
    {
        health = 16f;
        //moveDuration = Random.Range(1.5f, 3f);

        if (isLeft) startPos = EnemyManager.Ins.leftPos.position;
        else startPos = EnemyManager.Ins.rightPos.position;
    }

    void Update()
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

    void MoveToPos()
    {
        transform.DOMove(pos.position, moveDuration);
    }
}
