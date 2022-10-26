using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_GSP3 : MonoBehaviour
{
	public GameObject wingsPrefab1;
	public GameObject wingsPrefab2;
	public bool chooseWings1;

	//public ParticleSystem superEffect;

	private Player_GSP3 fighter;
	void Start()
	{
		fighter = Player_GSP3.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			GameObject wingsObj;
			if (chooseWings1)
			{
				wingsObj = Instantiate(wingsPrefab1, transform.position, Quaternion.identity, transform);
			}
			else
			{
				wingsObj = Instantiate(wingsPrefab2, transform.position, Quaternion.identity, transform);
			}

			Destroy(wingsObj, 7f);

			//superEffect.Play();
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}
	}
}
