using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_V4 : MonoBehaviour
{
    #region Singleton
    public static Player_V4 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_V4 body;
    public Animator anim;

    public GameObject shieldAura;
    public GameObject vfx_explosion;
    public GameObject vfx_ripple;

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
    private bool _canShoot = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        shieldAura.SetActive(false);
        vfx_ripple.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            GameManager_V4.Ins.canInteract = true;
            GameManager_V4.Ins.ShowTut();
        });
    }

    public void MoveEndGame()
	{
        _canShoot = false;
        transform.DOMove(new Vector3(0, -6, 0), 2f);
        movement.canMove = false;
    }

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

    public void StartShooting()
    {
        movement.canMove = true;
        shieldAura.SetActive(true);
        vfx_ripple.SetActive(false);
        StartCoroutine(IE_SpawnBullet());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG))
        {
            anim.SetBool("Fade", true);
            GameManager_V4.Ins.ShowRedVFX(true);
            Invoke(nameof(DeactiveFade), 1f);
        }
    }

    private void DeactiveFade()
	{
        anim.SetBool("Fade", false);
        GameManager_V4.Ins.ShowRedVFX(false);
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
}
