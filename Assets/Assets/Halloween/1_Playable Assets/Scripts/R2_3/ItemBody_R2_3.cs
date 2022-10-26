using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R2_3 : MonoBehaviour
{
	public AudioSource boostSound;

	private Player_R2_3 fighter;
	void Start()
	{
		fighter = Player_R2_3.Ins;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			if (!fighter._isBulletBoosted)
			{
				fighter.GetBulletBoost();
			}
			else
			{
				fighter.GetBooster();
			}

			Destroy(collision.gameObject);
		}
	}
}
