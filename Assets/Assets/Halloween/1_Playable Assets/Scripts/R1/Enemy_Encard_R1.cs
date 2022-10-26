using DG.Tweening;
using UnityEngine;

public class Enemy_Encard_R1 : MonoBehaviour
{
    public Animator anim;

    public Transform initPos;
    public int curMove;

	void Start()
	{
        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
    }

	public void MoveToPos(Transform pos)
    {
        transform.DOMove(pos.position, 1f);
    }
}
