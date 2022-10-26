using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R2P_3_1 : MonoBehaviour
{
	private Player_R2P_3_1 fighter;

	void Start()
	{
		fighter = Player_R2P_3_1.Ins;
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
