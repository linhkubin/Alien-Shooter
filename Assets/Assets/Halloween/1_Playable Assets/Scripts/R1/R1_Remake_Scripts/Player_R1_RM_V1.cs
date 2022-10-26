using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player_R1_RM_V1 : MonoBehaviour
{
    #region Singleton
    public static Player_R1_RM_V1 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_R1 body;
    public Animator anim;

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

    [HideInInspector]
    public bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    [HideInInspector]
    public bool _isBulletBoosted = false;

    private bool _canShoot = true;

    public Collider col;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        vfx_ripple.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            GameManager_R1_RemakeV1.Ins.canInteract = true;
            GameManager_R1_RemakeV1.Ins.ShowTut();
        });
    }

    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
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
        vfx_ripple.SetActive(false);
        StartCoroutine(IE_SpawnBullet());
    }

    public void TurnOffBullet()
    {
        _canShoot = false;
        movement.canMove = false;
        col.enabled = false;
        Invoke(nameof(MoveToWin), 1f);
    }

    public void MoveToWin()
    {
        transform.DOMove(new Vector3(0, -5, 0), 1f).OnComplete(() => {
            transform.DOMoveY(13, 1f).OnComplete(() => {
                EndCard_R1.Ins.SetupEndCard();

                Ins = null;
                Destroy(gameObject);
            });
        });
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG))
        {
            anim.SetBool("Fade", true);
            GameManager_R1_RemakeV1.Ins.ShowRedVFX(true);
            Invoke(nameof(DeactiveFade), 1f);
        }
    }

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        GameManager_R1_RemakeV1.Ins.ShowRedVFX(false);
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
