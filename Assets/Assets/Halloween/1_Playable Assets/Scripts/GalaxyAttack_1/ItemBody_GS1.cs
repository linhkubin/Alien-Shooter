using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_GS1 : MonoBehaviour
{
	//public GameObject wingsPrefab1;
	//public GameObject wingsPrefab2;
	//public bool chooseWings1;

	//public ParticleSystem superEffect;

	private Player_GS1 fighter;
	void Start()
	{
		fighter = Player_GS1.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			//GameObject wingsObj;
			//if (chooseWings1)
			//{
			//	wingsObj = Instantiate(wingsPrefab1, transform.position, Quaternion.identity, transform);
			//}
			//else
			//{
			//	wingsObj = Instantiate(wingsPrefab2, transform.position, Quaternion.identity, transform);
			//}

			//Destroy(wingsObj, 10f);

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
