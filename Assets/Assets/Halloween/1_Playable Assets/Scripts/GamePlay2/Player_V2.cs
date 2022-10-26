using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_V2 : MonoBehaviour
{
    #region Singleton
    public static Player_V2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_V2 body;

    public GameObject shieldAura;
    public GameObject vfx_explosion;
    public GameObject vfx_ripple;
    public Transform[] bulletPos2;
    public Transform[] bulletPos5;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;

    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    private bool _canShoot = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        shieldAura.SetActive(false);
        //vfx_ripple.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(()=> { 
            GameManager_V2.Ins.canInteract = true;
            GameManager_V2.Ins.ShowTut();
        });
    }

    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void StartShooting()
    {
        //movement.canMove = true;
        shieldAura.SetActive(true);
        StartCoroutine(IE_SpawnBullet());
    }

	public void WaitToWin()
	{
        //body.shieldAura.UpdateColor(Color.green);
		movement.canMove = false;
        _canShoot = false;
		Invoke(nameof(MoveWinGame), 1f);
		GameManager_V2.Ins.EndGame();
	}

	void MoveWinGame()
	{
        transform.DOMove(new Vector3(0, -7f, 0), 1f);
	}

	//private void OnTriggerEnter2D(Collider2D collision)
 //   {
 //       if (collision.CompareTag(Constant.ENEMY_TAG))
 //       {            
 //           Instantiate(vfx_explosion, transform.position, Quaternion.identity);
 //           crashSound.Play();

 //           GameManager_V2.Ins.endGame = true;
 //           Ins = null;
 //           Destroy(gameObject);
 //       }
 //   }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else
        {
            foreach (Transform t in bulletPos5)
            {
                objectPooling.SpawnFromPool(Constant.BULLET_TAG, t.position, Quaternion.identity);
            }
        }
        shootSound.Play();

        if (_canShoot) StartCoroutine(IE_SpawnBullet());
    }
}
