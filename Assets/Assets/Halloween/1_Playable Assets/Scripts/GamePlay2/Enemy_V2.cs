using UnityEngine;
using DG.Tweening;
using System;

public class Enemy_V2 : SpaceShip_V2
{
    [HideInInspector] public Vector3[] path;

    [HideInInspector] public Transform targetPos;
    [HideInInspector] public bool canMove = false;
    [HideInInspector] public float speed = 5f;

    private bool reachedStartPos = false;

    public bool isFinal;

    void Start()
    {
        health = 6f;
    }

  //  void Update()
  //  {
  //      if (canMove && isFinal)
  //      {
		//	if (!reachedStartPos)
		//	{
		//		float distance = Vector3.Distance(transform.position, targetPos.position);
		//		if (distance > 0.25f)
		//		{
		//			transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed * Time.deltaTime);
		//		}
		//		else
		//		{
		//			reachedStartPos = true;
		//			Destroy(gameObject, 2f);
		//		}
		//	}
		//}
  //  }

    public void FollowPath(Transform[] pathPoints, float time)
	{
        path = new Vector3[pathPoints.Length];

        for(int i = 0; i<path.Length; i++)
		{
            path[i] = pathPoints[i].position;
		}

        transform.DOPath(path, time, PathType.CatmullRom, PathMode.TopDown2D).OnWaypointChange(SetDirection).SetEase(Ease.Linear);
	}

	private void SetDirection(int value)
	{
        Vector3 dir = path[value] - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.DORotateQuaternion(Quaternion.AngleAxis(angle - 90, Vector3.forward), 0.25f);
    }
}
