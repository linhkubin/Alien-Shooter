using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_R2 : MonoBehaviour
{
	public AudioSource coinSound;
	public AudioSource boostSound;

	private Player_R2_1 fighter;
	void Start()
	{
		fighter = Player_R2_1.Ins;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.BOOSTER_TAG))
		{
			Destroy(collision.gameObject);
			fighter.GetBooster();
		}

		else if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			fighter.GetBulletBoost();
			collision.gameObject.SetActive(false);
		}
	}
}
