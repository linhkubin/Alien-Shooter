using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Luna.Unity;

public class Player_R1_RM_2 : MonoBehaviour
{
    #region Singleton
    public static Player_R1_RM_2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Animator anim;
    public Animator animGun;
    public Animator animGunLeft;
    public Animator animGunRight;
    public GameObject galaShip;
    public GameObject mainShip_sprite;

    public FighterMovement movement;
    public ItemBody_R1_Remake_V2 body;

    public Transform[] bulletPos5_gala;

    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos5;

    //-----------Sound------------
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;
    private GameManager_R1_Remake_2 gameManager;


    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    public bool _isBulletBoosted = false;

    private bool _canShoot = false;
    private bool _canShootGala = true;

    public GameObject RippleEffect;
    public bool isDead = false;
    public Transform tf_Player;
    public float health;
    public AudioSource sourceDieSound;
    public int deadCount;
    public Collider col;

    public bool immortal = false;

    public bool cantTakeDmgAfterEndgame = false;

    void Start()
    {
        tf_Player = transform;

        objectPooling = ObjectPooling.Ins;
        gameManager = GameManager_R1_Remake_2.Ins;

        galaShip.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-7, 1f).OnComplete(() => {
            gameManager.canInteract = true;
            RippleEffect.SetActive(true);
            gameManager.ShowTut();
        });
    }

    private void Update()
    {
        if(cantTakeDmgAfterEndgame == true)
        {
            health = 100000000;
        }
    }

    #region ROUND_1
    public void StartShooting()
    {
        movement.canMove = true;
        
        StartCoroutine(IE_SpawnGalaBullet());
        
    }
    public void StartShooting2()
    {
        movement.canMove = true;
        StartCoroutine(IE_SpawnBullet());
    }
    IEnumerator IE_SpawnGalaBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted && !_isBulletBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else if (!_isBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, bulletPos3[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, bulletPos3[1].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, bulletPos3[2].position, Quaternion.identity);
        }
        else
        {
            foreach (Transform t in bulletPos5)
            {
              
                objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, t.position, Quaternion.identity);
            }
        }
        shootSound.Play();

        if (_canShootGala && isDead == false) StartCoroutine(IE_SpawnGalaBullet());

        yield return new WaitForSeconds(spawnBulletDuration);
    }

    public void SwitchShip()
    {
        _canShootGala = false;
        boostedSound.Play();
        _canShoot = true;
        StartCoroutine(IE_SpawnBullet());
    }
    public void Revive()
    {
        immortal = true;
        tf_Player.DOMoveY(-13, 0.3f).OnComplete(() => {
            gameObject.SetActive(true);

            tf_Player.DOMoveY(-5, 0.7f).OnComplete(() => {
                StartShooting2();
                StartShooting();
                isDead = false;
            });
        });

        Invoke(nameof(_3sImmortal), 3f);
    }

    public void _3sImmortal()
    {
        immortal = false;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (deadCount < 1)
            {
                Revive();

            }
            sourceDieSound.Play();
            Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
            t.localScale = Vector3.one * 3f;
            gameObject.SetActive(false);
            isDead = true;
            deadCount++;
            if (deadCount > 1)
            {
                GameManager_R1_Remake_2.Ins.islose = true;
            }
        }
        //else
        //{
        //    healthBar.fillAmount = health / healthMax;
        //}
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
        //animGun.SetBool("TurnOff", true);
        //animGunLeft.SetBool("TurnOff", true);
        //animGunRight.SetBool("TurnOff", true);
        Invoke(nameof(MoveToWin), 1f);
    }

    public void MoveToWin()
    {
        transform.DOMove(new Vector3(0, -5, 0), 1f).OnComplete(() => {
            //transform.DOMoveY(13, 1f).OnComplete(() => {
            //    //EndCard_R1_1.Ins.SetupEndCard();
            //    Ins = null;
            //    Destroy(gameObject);
            //});
            RippleEffect.SetActive(true);
            gameManager.h_endCardPanel.SetActive(true);
            gameManager.v_endCardPanel.SetActive(true);
            Invoke(nameof(TurnOnCTA), 1f);

        });
    }
    public void TurnOnCTA()
    {
        StartCoroutine(IE_TurnOnCTA());
    }
    IEnumerator IE_TurnOnCTA()
    {
        yield return new WaitForSeconds(0.7f);

        gameManager.CTAClickH.SetActive(true);
        gameManager.CTAClickV.SetActive(true);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG))
        {
            
            if (GameManager_R1_Remake_2.Ins.startGame == true)
            {
                anim.SetBool("Fade", true);
                GameManager_R1_Remake_2.Ins.ShowRedVFX(true);
                if (immortal == false)
                {
                    TakeDamage(10);
                }
                Invoke(nameof(DeactiveFade), 1f);

            }
        }
    }
    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        GameManager_R1_Remake_2.Ins.ShowRedVFX(false);
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

        if (_canShoot && isDead == false) StartCoroutine(IE_SpawnBullet());
    }
    #endregion
}
