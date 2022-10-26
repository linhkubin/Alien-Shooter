using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R1_Remake_V2 : MonoBehaviour
{
	public AudioSource boostSound;

	private Player_R1_RM_2 fighter;
	void Start()
	{
		fighter = Player_R1_RM_2.Ins;
	}

	private void OnTriggerEnter(Collider collision)
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

		if (collision.CompareTag("ShipItem"))
		{
			fighter.SwitchShip();
			Destroy(collision.gameObject);
		}
	}
}
