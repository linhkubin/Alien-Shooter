using DG.Tweening;
using UnityEngine;

public class Enemy_EndCard_V5 : MonoBehaviour
{
    public Transform initPos;

    public void MoveToPos()
    {
        transform.DOMove(initPos.position, 1f);
    }
}
