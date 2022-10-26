using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Player_R2_3 : MonoBehaviour
{
    #region Singleton
    public static Player_R2_3 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_R2_3 body;
    public Animator anim;
    public GameObject ship;

    public GameObject vfx_explosion;

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
    private GameManager_R2_3 gameManager;

    private bool hasRevived = false, isAlive = true;

    [HideInInspector]
    public bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    [HideInInspector]
    public bool _isBulletBoosted = false;

    private bool _canShoot = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        gameManager = GameManager_R2_3.Ins;

        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            gameManager.canInteract = true;
            gameManager.ShowTut();
        });
    }

    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.12f;
        //Constant.BULLET_SPEED *= 1.2f;
    }

    public void GetBulletBoost()
    {
        boostedSound.Play();
        _isBulletBoosted = true;
        spawnBulletDuration = 0.15f;
        //Constant.BULLET_SPEED *= 1.1f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void StartShooting()
    {
        movement.canMove = true;
        StartCoroutine(IE_SpawnBullet());
    }

    public void TurnOffBullet()
    {
        EndCard_R2_3.Ins.ShowEndCard();
        Invoke(nameof(MoveToWin), 4f);
    }

    public void MoveToWin()
    {
        _canShoot = false;
        movement.canMove = false;
        transform.DOMove(new Vector3(0, -5, 0), 1f).OnComplete(()=> {
            gameManager.WaitToShowEndCard(0f);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) && isAlive && !gameManager.isWin)
        {
            //anim.SetBool("Fade", true);
            //gameManager.ShowRedVFX(true);
            //Invoke(nameof(DeactiveFade), 1f);
            isAlive = false;

            movement.canMove = false;
            _canShoot = false;
            ship.SetActive(false);
            Instantiate(vfx_explosion, transform.position, Quaternion.identity);
            crashSound.Play();
            if (!hasRevived)
			{
                hasRevived = true;
                Invoke(nameof(Revive), 0.5f);
            }
            else
			{
                gameManager.WaitToShowGameOver(0.75f);
            }
        }
    }

    private void DeactiveFade()
    {
        anim.SetBool("Fade", false);
        gameManager.ShowRedVFX(false);
    }

    private void Revive()
	{
        transform.position = new Vector3(0, -13f, 0);
        ship.SetActive(true);
        anim.SetBool("Fade", true);
        transform.DOMoveY(-5, 1f).OnComplete(() => {
            if (!gameManager.isWin)
			{
                movement.canMove = true;
                _canShoot = true;
                StartCoroutine(IE_SpawnBullet());
            }
            Invoke(nameof(DeactiveFade), 0.5f);
            isAlive = true;
        });
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
}
