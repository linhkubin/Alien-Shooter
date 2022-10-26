using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Luna.Unity;

public class Player_R1 : MonoBehaviour
{
    #region Singleton
    public static Player_R1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_R1 body;
    public Animator anim;
    public Animator animShip2;
    public Collider col;
    public Rigidbody rb;
    public GameObject vfx_explosion;
    public GameObject vfx_ripple;

    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos5;
    public Transform bigBulletPos1;
    public Transform bigBulletPos2;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;
    public AudioSource shootSound2;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;

    [HideInInspector]
    public bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    [HideInInspector]
    public bool _isBulletBoosted = false;

    private bool _canShoot = true;

    public float healthMax;
    public float health;
    public Image healthBar;
    public AudioSource sourceDieSound;
    public Transform tf_Player;
    public GameObject shipSprite1;
    public GameObject shipSprite2;

    public GameObject _ShieldAura;

    public int deadCount;
    public bool isDead;

    public float timingSpawnBigBullet;
    public float maxTimingSpawnBigBullet; 
    public float timingSpawnBullet;
    public float maxTimingSpawnBullet;

    void Start()
    {
        tf_Player = transform;
        objectPooling = ObjectPooling.Ins;
        vfx_ripple.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            GameManager_R1.Ins.canInteract = true;
            GameManager_R1.Ins.ShowTut();
        });

        health = healthMax;

    }

    private void Update()
    {
        if(GameManager_R1.Ins.startGame == true && GameManager_R1.Ins.isWin == false)
        {
            if (_canShoot)
            {
                timingSpawnBigBullet += Time.deltaTime;
                timingSpawnBullet += Time.deltaTime;
            }

            if (_canShoot && timingSpawnBigBullet >= maxTimingSpawnBigBullet)
            {
                timingSpawnBigBullet = 0;
                StartCoroutine(IE_SpawnBigBullet());
            }

            if (_canShoot && timingSpawnBullet >= maxTimingSpawnBullet)
            {
                timingSpawnBullet = 0;
                StartCoroutine(IE_SpawnBullet());
            }
        }
    }

    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        maxTimingSpawnBullet = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        

        shipSprite1.SetActive(false);
        shipSprite2.SetActive(true);
        anim = animShip2;
        _ShieldAura.SetActive(true);
        shipSprite2.transform.DOScale(0.2f, 0.5f).OnComplete(() => {
            shipSprite2.transform.DOScale(0.135f, 0.5f);
        }); ;
    }

    public void GetBulletBoost()
    {
        boostedSound.Play();

        _isBulletBoosted = true;
        spawnBulletDuration = 0.1f;
        maxTimingSpawnBullet = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        //Constant.BULLET_DAMAGE *= 2;
        shipSprite1.transform.DOScale(0.2f, 0.5f).OnComplete(() => {
            shipSprite1.transform.DOScale(0.135f, 0.5f);
        }); ;
    }

    public void StartShooting()
    {
        movement.canMove = true;
        vfx_ripple.SetActive(false);
    }

    public void TurnOffBullet()
    {
        _canShoot = false;
        movement.canMove = false;
        col.enabled = false;
        rb.detectCollisions = false;
        Invoke(nameof(MoveToWin), 1f);
        
    }

    public void MoveToWin()
	{
        tf_Player.DOMove(new Vector3(0, -5, 0), 1f).OnComplete(()=> {
            //transform.DOMoveY(13, 1f).OnComplete(() => {
            health = 10000;
            //Ins = null;
            vfx_ripple.SetActive(true);
            //EndCard_R1.Ins.SetupEndCard();
            GameManager_R1.Ins.winCard.SetActive(true);
            GameManager_R1.Ins.objectPool.SetActive(false);

            //Analytics.LogEvent(Analytics.EventType.EndCardShown, 1);
           
            //Destroy(gameObject);
            //});
        });
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG))
        {
            anim.SetBool("Fade", true);
            GameManager_R1.Ins.ShowRedVFX(true);
            TakeDamage(10);
            Invoke(nameof(DeactiveFade), 1f);
        }
    }

    public void Revive()
    {
        tf_Player.DOMoveY(-13, 0.3f).OnComplete(() => {
            gameObject.SetActive(true);
            
            transform.DOMoveY(-5, 0.7f).OnComplete(() => {
                StartShooting();
                isDead = false;
            });
        });
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if(deadCount < 1)
            {
                Revive();
            }
            sourceDieSound.Play();
            Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
            t.localScale = Vector3.one * 3f;
            gameObject.SetActive(false);
            isDead = true;
            deadCount++;
            if(deadCount > 1)
            {
                GameManager_R1.Ins.islose = true;
            }
        }
        //else
        //{
        //    healthBar.fillAmount = health / healthMax;
        //}
    }

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        GameManager_R1.Ins.ShowRedVFX(false);
    }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted && !_isBulletBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            //objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
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
                objectPooling.SpawnFromPool(Constant.BULLET_BLUE_TAG, t.position, t.transform.rotation);
            }
        }
        shootSound.Play();

        //if (_canShoot) StartCoroutine(IE_SpawnBullet());
    }


    IEnumerator IE_SpawnBigBullet()
    {
        yield return new WaitForSeconds(5f);

       
        objectPooling.SpawnFromPool(Constant.BULLET_BIG_TAG, bigBulletPos1.position, Quaternion.identity);
        objectPooling.SpawnFromPool(Constant.BULLET_BIG_TAG, bigBulletPos2.position, Quaternion.identity);
       
        shootSound2.Play();

        //if (_canShoot) StartCoroutine(IE_SpawnBigBullet());
    }

    
}
