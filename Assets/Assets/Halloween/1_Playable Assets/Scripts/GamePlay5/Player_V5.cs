using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_V5 : MonoBehaviour
{
    #region Singleton
    public static Player_V5 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public GameObject galaShip;
    public GameObject mainShip;
    public GameObject mainShip_sprite;

    public FighterMovement movement;
    public ItemBody_V5 body;

    public GameObject vfx_explosion;

    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos5;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;
     
    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    private bool _isBulletBoosted = false;

    private bool _canShoot = false;
    private bool _canShootGala = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;

        mainShip.SetActive(false);
        galaShip.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            GameManager_V5.Ins.canInteract = true;
            GameManager_V5.Ins.ShowTut();
        });
    }

	#region ROUND_1
	public void StartShooting()
    {
        movement.canMove = true;
        StartCoroutine(IE_SpawnGalaBullet());
    }
    IEnumerator IE_SpawnGalaBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        foreach (Transform t in bulletPos5)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, t.position, Quaternion.identity);
        }
        shootSound.Play();

        if (_canShootGala) StartCoroutine(IE_SpawnGalaBullet());
    }

    public void SwitchShip()
	{
        galaShip.SetActive(false);
        mainShip.SetActive(true);

        mainShip_sprite.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(()=> {
            mainShip_sprite.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        });

        _canShootGala = false;

        boostedSound.Play();
        _canShoot = true;
        StartCoroutine(IE_SpawnBullet());
	}

    #endregion

    #region ROUND_2
    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void GetBulletBoost()
    {
        boostedSound.Play();
        _isBulletBoosted = true;
        spawnBulletDuration = 0.15f;
        Constant.BULLET_SPEED *= 1.1f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void TurnOffBullet()
	{
        _canShoot = false;
        movement.canMove = false;
    }

    public void MoveEndGame()
    {
        transform.DOMove(new Vector3(0, -5, 0), 1.5f);
	}

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted && !_isBulletBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else if (!_isBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[1].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[2].position, Quaternion.identity);
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
	#endregion
}
