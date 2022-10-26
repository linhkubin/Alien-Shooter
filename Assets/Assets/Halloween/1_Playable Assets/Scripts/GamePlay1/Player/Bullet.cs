using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[HideInInspector]
	public bool isBoosted = false;
	[HideInInspector]
	public Transform boostedPos;

	private bool onActive = true;
	private bool moveToInit = false;

    void OnEnable()
    {
        onActive = true;
	}

	void OnDisable()
	{
		onActive = false;
		isBoosted = false;
		moveToInit = false;
	}

	void Update()
    {
		if (onActive)
		{
			if (!isBoosted)
			{
				transform.Translate(Vector3.up * Constant.BULLET_SPEED * Time.deltaTime);

				if (transform.position.y > 13f)
				{
					ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
					onActive = false;
				}
			}
			else
			{
				if (!moveToInit)
				{
					moveToInit = true;
					transform.localScale = Vector3.one * 0.75f;
					transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
					transform.DOMove(boostedPos.position, 0.15f).SetEase(Ease.Linear).OnComplete(() => {
						isBoosted = false;
					});
				}
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
