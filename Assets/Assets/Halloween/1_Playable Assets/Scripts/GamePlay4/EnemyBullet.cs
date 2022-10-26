using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private bool onActive = true;

	void OnEnable()
	{
		onActive = true;
	}

	void OnDisable()
	{
		onActive = false;
	}

	void Update()
	{
		if (onActive)
		{
			transform.Translate(Vector2.down * 4f * Time.deltaTime);	

			if (transform.position.y < -13f)
			{
				ObjectPooling.Ins.DespawnObject(Constant.ENEMY_BULLET_TAG, gameObject);
				onActive = false;
			}
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.PLAYER_TAG) /*|| collision.CompareTag(Constant.PLAYER_SPRITES_TAG)*/)
		{
			ObjectPooling.Ins.DespawnObject(Constant.ENEMY_BULLET_TAG, gameObject);
			ObjectPooling.Ins.SpawnFromPool(Constant.SPARK2_TAG, transform.position, Quaternion.identity);
			onActive = false;
		}
	}
}
