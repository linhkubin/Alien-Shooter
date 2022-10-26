using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_GSP2 : MonoBehaviour
{
	public GameObject wingsPrefab;
	public ParticleSystem superEffect;

	private Player_GSP2 fighter;
	void Start()
	{
		fighter = Player_GSP2.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			GameObject wingsObj = Instantiate(wingsPrefab, transform.position, Quaternion.identity, transform);
			Destroy(wingsObj, 7f);

			superEffect.Play();
			fighter.GetBooster();
			Destroy(collision.gameObject);
		}
	}
}
