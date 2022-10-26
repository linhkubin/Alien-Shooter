using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_V5 : MonoBehaviour
{
	public AudioSource coinSound;
	public AudioSource boostSound;

	public GameObject wingsPrefab;
	public ParticleSystem superEffect;
	public ShieldAura shieldAura;

	private Player_V5 fighter;
	void Start()
	{
		fighter = Player_V5.Ins;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.BOOSTER_TAG))
		{
			Destroy(collision.gameObject);
			fighter.GetBooster();

			GameObject wingsObj = Instantiate(wingsPrefab, transform);
			Destroy(wingsObj, 5f);
			superEffect.Play();
		}

		else if (collision.CompareTag(Constant.BULLET_BOOST_TAG))
		{
			fighter.GetBulletBoost();
			collision.gameObject.SetActive(false);
		}
	}
}
