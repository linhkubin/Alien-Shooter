using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_V3 : MonoBehaviour
{
	public AudioSource coinSound;
	public AudioSource boostSound;

	public GameObject wingsPrefab;
	public ParticleSystem superEffect;
	public ShieldAura shieldAura;

	private Player_V3 fighter;
	void Start()
	{
		fighter = Player_V3.Ins;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.BOOSTER_TAG))
		{
			Destroy(collision.gameObject);
			fighter.GetBooster();

			GameObject wingsObj = Instantiate(wingsPrefab, transform);
			//wingsObj.GetComponent<WingController>().SetSkin("a");
			//wingsObj.GetComponent<WingsNoSpine>().ActiveWings();
			superEffect.Play();

			//shieldAura.UpdateColor(Color.yellow);
		}

		else if (collision.CompareTag(Constant.COIN_TAG))
		{
			ObjectPooling.Ins.DespawnObject(Constant.COIN_TAG, collision.gameObject);
			collision.gameObject.SetActive(false);
		}
	}
}
