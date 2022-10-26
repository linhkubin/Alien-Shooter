using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody_V2 : MonoBehaviour
{
	public AudioSource coinSound;
	public AudioSource boostSound;

	public GameObject wingsPrefab;
	public ParticleSystem superEffect;
	public ShieldAura shieldAura;

	private Player_V2 fighter;
	void Start()
	{
		fighter = Player_V2.Ins;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag(Constant.BOOSTER_TAG))
		{
			Destroy(collision.gameObject);
			fighter.GetBooster();

			GameObject wingsObj = Instantiate(wingsPrefab, transform);
			//wingsObj.GetComponent<WingController>().SetSkin("a");
			Destroy(wingsObj, 7f);

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
