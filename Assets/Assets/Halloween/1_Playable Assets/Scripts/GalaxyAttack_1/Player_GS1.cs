using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GS1 : MonoBehaviour
{
    #region Singleton
    public static Player_GS1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Animator anim1, anim2;
    public GameObject galaShip;
    public GameObject mainShip;
    public GameObject mainShip_sprite;

    public FighterMovement movement;
    public ItemBody_GS1 body;

    public GameObject vfx_Ripple;

    public Transform[] galaBulletPos5;

    public Transform[] bulletPos2;
    public Transform[] bulletPos5;
    public Transform[] bulletPos5New;
    public Transform[] Gun;
    public Transform pos5Center;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.25f;

    private ObjectPooling objectPooling;

    public bool _isBoosted = false; //Kiem tra xem co dang an booster ko 

    public bool _canShoot = false;
    public bool _canShootGala = true;

    private bool playGun;

    public GameObject Drone;

    public int health;
    public int maxHealth;
    public AudioSource sourceDieSound;
    private GameManager_GS1 gameManager;

    public Collider col;
    public Rigidbody rb;
    public bool isDead = false;
    void Start()
    {
        gameManager = GameManager_GS1.Ins;
        playGun = true;
        health = maxHealth;
        objectPooling = ObjectPooling.Ins;

        mainShip.SetActive(false);
        galaShip.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-7.5f, 1f).OnComplete(() => {
            GameManager_GS1.Ins.canInteract = true;
            GameManager_GS1.Ins.ShowTut();
            vfx_Ripple.SetActive(true);
        });
    }

	private void OnTriggerEnter(Collider collision)
	{
        if (gameManager.startGame == true)
        {
            if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG))
            {
                anim1.SetBool("Fade", true);
                anim2.SetBool("Fade", true);
                GameManager_GS1.Ins.ShowRedVFX(true);
                Invoke(nameof(DeactiveFade), 1f);
                crashSound.PlayOneShot(crashSound.clip);
            //TakeDamage(10);
            //Drone.SetActive(false);
            //gameManager.isLose = true;
        }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            isDead = true;
            sourceDieSound.Play();
            GameObject t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
            if (t != null) t.transform.localScale = Vector3.one * 3f;
            gameObject.SetActive(false);
        }
        //else
        //{
        //    healthBar.fillAmount = health / healthMax;
        //}
    }

    private void DeactiveFade()
    {
        anim1.SetBool("Fade", false);
        anim2.SetBool("Fade", false);
        GameManager_GS1.Ins.ShowRedVFX(false);
    }

    #region ROUND_1
    public void StartShooting()
    {
        vfx_Ripple.SetActive(false);
        movement.canMove = true;
        StartCoroutine(IE_SpawnGalaBullet());
        DelayGun();
    }
    IEnumerator IE_SpawnGalaBullet()
    {
        
        yield return new WaitForSeconds(spawnBulletDuration);

        foreach (Transform t in galaBulletPos5)
        {
            if (gameManager.PauseGame == false)
            {
                objectPooling.SpawnFromPool(Constant.BULLET_GALA_TAG, t.position, Quaternion.identity);
            }
        }
        shootSound.Play();

        if (_canShootGala) StartCoroutine(IE_SpawnGalaBullet());
    }

    public void SwitchShip()
    {
        galaShip.SetActive(false);
        mainShip.SetActive(true);

        mainShip_sprite.transform.DOScale(new Vector3(0.25f, 0.25f, 0.25f), 0.5f).OnComplete(() => {
            mainShip_sprite.transform.DOScale(new Vector3(0.13f, 0.13f, 0.13f), 0.5f);
        });

        _canShootGala = false;

        _isBoosted = false;
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
        spawnBulletDuration = 0.13f;
        Constant.BULLET_SPEED *= 1.2f;
        Constant.BULLET_DAMAGE *= 2;
    }

    public void GetBulletBoost()
    {
        _isBoosted = true;
        boostedSound.Play();
        spawnBulletDuration = 0.2f;
        Constant.BULLET_SPEED *= 1.1f;
        Constant.BULLET_DAMAGE *= 2;
    }

    public void WaitToTurnOffBullet(float time)
    {
        col.enabled = false;

        rb.detectCollisions = false;

        Invoke(nameof(TurnOffBullet), time);
    }

    public void TurnOffBullet()
    {
        _canShoot = false;
        movement.canMove = false;

        MoveEndGame();
    }

    public void MoveEndGame()
    {
        transform.DOMove(new Vector3(0, -8, 0), 1f).OnComplete(() => {
            GameManager_GS1.Ins.ShowEndCard();
        }); ;
    }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted)
        {
            //if (gameManager.PauseGame == false)
            //{
                objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
                objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
            //}
                
        }
        else
        {
            Drone.SetActive(true);

            foreach (Transform t in bulletPos5)
            {
                //if (gameManager.PauseGame == false)
                //{
                    GameObject b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos5Center.position, Quaternion.identity);
                if(b != null)
                {
                    b.GetComponent<Bullet_R1P>().isBoosted = true;
                    b.GetComponent<Bullet_R1P>().boostedPos = t;
                }
                    
                //}
            }

           
            if (playGun == true)
            {
                GameObject GunLeft = objectPooling.SpawnFromPool(Constant.BULLET_GUN_LEFT, Gun[0].position, Quaternion.Euler(new Vector3(0, 0, 20f)));
                GameObject GunRight = objectPooling.SpawnFromPool(Constant.BULLET_GUN_RIGHT, Gun[1].position, Quaternion.Euler(new Vector3(0, 0, -20f)));

                if(GunLeft!= null)
                {
                    GunLeft.GetComponent<BulletHomming>().setStartPos(Gun[0]);
                }

                if(GunRight != null)
                {
                    GunRight.GetComponent<BulletHomming>().setStartPos(Gun[1]);
                }
            }

        }

        shootSound.Play();

        if (_canShoot) StartCoroutine(IE_SpawnBullet());
        
    }

    public void DelayGun()
    {
        StartCoroutine(IE_DelayGun());
    }
    IEnumerator IE_DelayGun()
    {
        yield return new WaitForSeconds(1.1f);
        playGun = true;

        if (playGun == true)
        {
            yield return new WaitForSeconds(1.1f);
            playGun = false;
        }
      
        if(playGun == false)
        {
            StartCoroutine(IE_DelayGun());
        }
    }

    #endregion
}
