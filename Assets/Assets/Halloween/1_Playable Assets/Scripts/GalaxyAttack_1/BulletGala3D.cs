using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGala3D : MonoBehaviour
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
			transform.Translate(Vector3.up * 15f * Time.deltaTime);

			if (transform.position.y > 13f)
			{
				ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
				onActive = false;
			}
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.ENEMY_TAG))
		{
			ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
			ObjectPooling.Ins.SpawnFromPool(Constant.SPARK_TAG, transform.position, Quaternion.identity);
			onActive = false;
		}
	}
}
