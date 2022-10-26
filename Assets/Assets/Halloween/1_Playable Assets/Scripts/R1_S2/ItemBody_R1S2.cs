using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R1S2 : MonoBehaviour
{
	private Player_R1S2 fighter;
	void Start()
	{
		fighter = Player_R1S2.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}
	}

	private void OnTriggerStay(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}
	}




}
