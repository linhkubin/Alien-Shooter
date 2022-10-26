using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R1P : MonoBehaviour
{
	public AudioSource boostSound;

	private Player_R1P fighter;
	void Start()
	{
		fighter = Player_R1P.Ins;
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
