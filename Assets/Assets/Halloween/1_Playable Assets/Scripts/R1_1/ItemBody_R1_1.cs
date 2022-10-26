using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R1_1 : MonoBehaviour
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
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}

		if (collision.CompareTag("ShipItem"))
		{
			fighter.SwitchShip();
			Destroy(collision.gameObject);
		}
	}

	private void OnTriggerStay(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			//superEffect.Play();
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}

		if (collision.CompareTag("ShipItem"))
		{
			fighter.SwitchShip();
			Destroy(collision.gameObject);
		}
	}
}
