using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBodyR1BLH : MonoBehaviour
{
	public AudioSource boostSound;

	private PlayerR1BLH fighter;
	void Start()
	{
		fighter = PlayerR1BLH.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			//if (!fighter._isBulletBoosted)
			//{
			//	fighter.GetBulletBoost();
			//}
			//else
			//{
			fighter.GetBooster();
			//}

			Destroy(collision.gameObject);
		}
	}
}
