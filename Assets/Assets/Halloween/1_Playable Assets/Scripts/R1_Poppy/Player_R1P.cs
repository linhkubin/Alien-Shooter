using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_R1P : MonoBehaviour
{
    #region Singleton
    public static Player_R1P Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_R1P body;
    public Animator anim;
    public Collider col;

    public GameObject vfx_explosion;
    public GameObject vfx_ripple;

    public Transform bulletPos1;
    public Transform[] bulletPos2;
    public Transform[] bulletPos3;
    public Transform[] bulletPos5;
    public Transform pos5Center;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;
    private GameManager_R1P gameManager;
    private EndCard_R1P endCard;

    private bool _canShoot = true;
    private bool _haveRevive = false;

    [HideInInspector]
    public bool[] boostedList = new bool[3];
    private int boostedIdx = -1;

  
    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        gameManager = GameManager_R1P.Ins;
        endCard = EndCard_R1P.Ins;

        vfx_ripple.SetActive(true);
        for (int i = 0; i < boostedList.Length; i++)
        {
            boostedList[i] = false;
        }

        transform.DOMoveY(-5, 1f).OnComplete(() =>
        {
            gameManager.canInteract = true;
            gameManager.ShowTut();
        });

        boostedIdx = 0;
    }

    public void GetBooster()
    {
        boostedSound.Play();

        //if (boostedIdx < boostedList.Length)
        //    boostedIdx++;
        //boostedList[boostedIdx] = true;

        boostedIdx = 2;

        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
    }

    public void StartShooting()
    {
        movement.canMove = true;
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
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag("Boss"))
        {
            if (!gameManager.isWin)
			{
                col.enabled = false;
                movement.canMove = false;
                _canShoot = false;
                Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
                t.localScale = Vector3.one * 3f;
                gameObject.SetActive(false);
                transform.position = new Vector3(0, -13f, 0);

                if (!_haveRevive)
				{
                    _haveRevive = true;
                    Invoke(nameof(Revive), 0.5f);
                }
                else
				{
                    endCard.WaitToShowEndCard(0.5f, false);
				}
            }
   //         else
			//{
   //             anim.SetBool("Fade", true);
   //             gameManager.ShowRedVFX(true);
   //             Invoke(nameof(DeactiveFade), 1f);
   //         }
        }
    }

    private void Revive()
	{
        gameObject.SetActive(true);

        anim.SetBool("Fade", true);
        transform.DOMoveY(-5, 1f).OnComplete(() => {
            Invoke(nameof(DeactiveFade), 1f);
            Invoke(nameof(ActiveCollider), 1f);
            _canShoot = true;
            StartShooting();
        });
    }

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        //gameManager.ShowRedVFX(false);
    }

    private void ActiveCollider()
    {
        col.enabled = true;
    }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (boostedIdx == -1)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos1.position, Quaternion.identity);
        }
        else if (boostedIdx == 0)
		{
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
        }
        else if (boostedIdx == 1)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[1].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos3[2].position, Quaternion.identity);
        }
        else if(boostedIdx == 2)
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
