using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletEnemy_R1P : MonoBehaviour
{
	public Vector3 targetPos;

	private bool onActive = true;

	void OnEnable()
	{
		onActive = true;
	}

	void OnDisable()
	{
		onActive = false;
	}

	//void Update()
	//{
	//	if (onActive)
	//	{
	//		if (transform.position.y < -13f)
	//		{
				
	//		}
	//	}
	//}

	public void MoveToPos()
	{
		transform.DOMove(targetPos, Random.Range(1f, 2f)).OnComplete(()=>
		{
			ObjectPooling.Ins.DespawnObject(Constant.ENEMY_BULLET_TAG, gameObject);
			onActive = false;
		});
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.PLAYER_TAG))
		{
			ObjectPooling.Ins.DespawnObject(Constant.ENEMY_BULLET_TAG, gameObject);
			ObjectPooling.Ins.SpawnFromPool(Constant.SPARK_TAG, transform.position, Quaternion.identity);
			onActive = false;
		}
	}
}
