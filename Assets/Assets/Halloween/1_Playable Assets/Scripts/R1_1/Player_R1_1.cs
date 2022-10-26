using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_R1_1 : MonoBehaviour
{
    #region Singleton
    public static Player_R1_1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Animator anim;
    public GameObject galaShip;
    public GameObject mainShip;
    public GameObject mainShip_sprite;

    public FighterMovement movement;
    public ItemBody_R1_1 body;

    public Transform[] bulletPos5_gala;

    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos5;
    public Transform pos5Center;

    //-----------Sound------------
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;
    private GameManager_R1_1 gameManager;


    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    public bool _isBulletBoosted = false;

    private bool _canShoot = false;
    private bool _canShootGala = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        gameManager = GameManager_R1_1.Ins;

        mainShip.SetActive(false);
        galaShip.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-7, 1f).OnComplete(() => {
            gameManager.canInteract = true;
            gameManager.ShowTut();
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

        foreach (Transform t in bulletPos5_gala)
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

        mainShip_sprite.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).OnComplete(() => {
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
        Invoke(nameof(MoveToWin), 1f);
    }

    public void MoveToWin()
    {
        transform.DOMove(new Vector3(0, -5, 0), 1f).OnComplete(() => {
            transform.DOMoveY(13, 1f).OnComplete(() => {
                EndCard_R1_1.Ins.SetupEndCard();

                Ins = null;
                Destroy(gameObject);
            });
        });
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG))
        {
            anim.SetBool("Fade", true);
            GameManager_R1_1.Ins.ShowRedVFX(true);
            Invoke(nameof(DeactiveFade), 1f);
        }
    }
    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        GameManager_R1_1.Ins.ShowRedVFX(false);
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
                Bullet b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos5Center.position, Quaternion.identity).GetComponent<Bullet>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        shootSound.Play();

        if (_canShoot) StartCoroutine(IE_SpawnBullet());
    }
    #endregion
}
