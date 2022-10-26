using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_R1S2 : MonoBehaviour
{
    #region Singleton
    public static Player_R1S2 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public Animator anim;

    public FighterMovement movement;
    public ItemBody_R1S2 body;

    public GameObject shield;
    public GameObject vfx_Ripple;

    public Transform bulletPos1;
    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos4;
    public Transform[] bulletPos5;
    public Transform[] bulletPos7;
    public Transform[] bulletPos9;
    public Transform[] bulletPos11;

    public Transform pos11Center;
    public Transform pos9Center;
    public Transform pos7Center;
    public Transform pos5Center;
    public Transform pos3Center;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.15f;
    private float spawnBigBulletDuration = 2f;

    private ObjectPooling objectPooling;

    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 

    private bool _canShoot = true;

    public float healthMax;
    public float health;
    public Image healthBar;
    public AudioSource sourceDieSound;
    public GameObject WinCard;

    public int bulletCount = 1;

    public bool canCollision;

    public GameObject ShipEndcard;
    public Collider col;

    public GameObject EndcardBoardNumber;

    void Start()
    {
        canCollision = true;

        health = healthMax;
        //healthBar.fillAmount = healthMax / 25f;


        objectPooling = ObjectPooling.Ins;

        _isBoosted = false;

        transform.DOMoveY(-6, 1f).OnComplete(() => {
            GameManager_R1S2.Ins.canInteract = true;
            GameManager_R1S2.Ins.ShowTut();
        });
    }

    private void Update()
    {
        if(bulletCount <= 0)
        {
            bulletCount = 1;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG) || collision.CompareTag(Constant.BOSS_BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE_PLAYER);
            anim.SetBool("Fade", true);
            GameManager_R1S2.Ins.ShowRedVFX(true);
            Invoke(nameof(DeactiveFade), 1f);
            crashSound.PlayOneShot(crashSound.clip);
        }

        if (collision.CompareTag(Constant.PLUS1_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount += 1;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS2_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount += 2;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS3_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount += 3;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS5_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount += 5;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUSX2_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount *= 2;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS10_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount += 10;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);

            }
        }
        else if (collision.CompareTag(Constant.MINUS10_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount -= 10;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                
            }
        }
        else if (collision.CompareTag(Constant.MINUS5_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount -= 5;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                
            }
        }
        else if (collision.CompareTag(Constant.MINUS66_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount -= 66;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                
            }
        }
        else if (collision.CompareTag(Constant.X0_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                bulletCount *= 0;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                
            }
        }

        if (collision.CompareTag(Constant.RESET_BULLET_TAG))
        {
            canCollision = true;

            collision.GetComponent<Collider>().enabled = false;
        }

    }
    public virtual void TakeDamage(int damage)
    {
        if (GameManager_R1S2.Ins.startGame == true)
        {
            health -= damage;

            if (health <= 0)
            {
                GameManager_R1S2.Ins.isLose = true;
                //sourceDieSound.Play();
                Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
                t.localScale = Vector3.one * 3f;
                gameObject.SetActive(false);
            }
            else
            {
                healthBar.fillAmount = health / healthMax;
            }
        }
    }

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        GameManager_R1S2.Ins.ShowRedVFX(false);
    }

    #region ROUND_1
    public void StartShooting()
    {
        vfx_Ripple.SetActive(false);
        movement.canMove = true;
        StartCoroutine(IE_SpawnBullet());
    }

    #endregion

    #region ROUND_2
    public void GetBooster()
    {
        shield.SetActive(true);
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void WaitToTurnOffBullet(float time)
    {
        Invoke(nameof(TurnOffBullet), time);
    }

    public void TurnOffBullet()
    {
        _canShoot = false;
        movement.canMove = false;
        col.enabled = false;
        MoveEndGame();

    }

    public void MoveEndGame()
    {
        StartCoroutine(IE_MoveEndGame());
    }

    IEnumerator  IE_MoveEndGame()
    {
        yield return new WaitForSeconds(1f);
        _canShoot = false;
        transform.DOMove(new Vector3(0, 25, 0), 1.5f).OnComplete(() => {
            
            //WinCard.SetActive(true);

            EnemyManager_R1S2.Ins.SetupEnemyRound2();

            EndcardBoardNumber.SetActive(true);

            EndcardBoardNumber.transform.DOMove(new Vector3(0, 4.75f, 0), 1f);

            ShipEndcard.transform.DOMove(new Vector3(0, -7, 0), 1f).OnComplete(()=>{

                GameManager_R1S2.Ins.ShowEndCard();

                
            });
        });
    }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (bulletCount == 1)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos1.position, Quaternion.identity);
            //objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else if (bulletCount == 2)
        {

            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);

        }
        else if (bulletCount == 3)
        {
            foreach (Transform t in bulletPos3)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos3Center.position, Quaternion.identity).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        else if (bulletCount == 4)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos4[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos4[1].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos4[2].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos4[3].position, Quaternion.identity);
        }
        else if (bulletCount >= 5 && bulletCount < 7)
        {
            foreach (Transform t in bulletPos5)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos5Center.position, Quaternion.identity).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        else if (bulletCount >= 7 && bulletCount <= 7.5f)
        {
            foreach (Transform t in bulletPos7)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos7Center.position, t.transform.rotation).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        else if (bulletCount >= 7.5f && bulletCount <= 8f)
        {
            foreach (Transform t in bulletPos9)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos9Center.position, t.transform.rotation).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        else if (bulletCount >= 8.5f)
        {
            foreach (Transform t in bulletPos11)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos11Center.position, t.transform.rotation).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        shootSound.Play();

        if (_canShoot) StartCoroutine(IE_SpawnBullet());
    }
    #endregion
}
