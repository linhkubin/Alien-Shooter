using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GSP1 : MonoBehaviour
{
    #region Singleton
    public static Player_GSP1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_GSP1 body;
    public Animator anim;

    public GameObject shieldAura;
    public GameObject vfx_ripple;

    public Transform[] bulletPos2;
    public Transform[] bulletPos5;
    public Transform pos5Center;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;
    private GameManager_GSP gameManager;
    private EndCard_R1P endCard;

    private bool _canShoot = true;
    private bool _haveRevive = false;

    [HideInInspector]
    public bool getBoosted = false;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        gameManager = GameManager_GSP.Ins;
        endCard = EndCard_R1P.Ins;

        shieldAura.SetActive(false);
        vfx_ripple.SetActive(true);

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            gameManager.canInteract = true;
            gameManager.ShowTut();
        });
    }

    public void GetBooster()
    {
        boostedSound.Play();
        getBoosted = true;

        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
    }

    public void StartShooting()
    {
        movement.canMove = true;
        shieldAura.SetActive(true);
        vfx_ripple.SetActive(false);
        StartCoroutine(IE_SpawnBullet());
    }

    public void WaitToTurnOffBullet(float time)
    {
        Invoke(nameof(TurnOffBullet), time);
    }

    public void TurnOffBullet()
    {
        _canShoot = false;
        movement.canMove = false;
        Invoke(nameof(MoveToWin), 1f);
    }

    public void MoveToWin()
    {
        transform.DOMove(new Vector3(0, -5, 0), 1f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG))
        {
			anim.SetBool("Fade", true);
			gameManager.ShowRedVFX(true);
			Invoke(nameof(DeactiveFade), 1f);
            crashSound.PlayOneShot(crashSound.clip);
        }
	}

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        gameManager.ShowRedVFX(false);
    }


    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!getBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else
        {
            foreach (Transform t in bulletPos5)
            {
                Bullet_R1P b = objectPooling.SpawnFromPool(Constant.BULLET_TAG, pos5Center.position, Quaternion.identity).GetComponent<Bullet_R1P>();
                b.isBoosted = true;
                b.boostedPos = t;
            }
        }
        shootSound.Play();

        if (_canShoot) StartCoroutine(IE_SpawnBullet());
    }
}
