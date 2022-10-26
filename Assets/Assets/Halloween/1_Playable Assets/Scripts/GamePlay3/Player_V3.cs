using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player_V3 : MonoBehaviour
{
    #region Singleton
    public static Player_V3 Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;
    public ItemBody_V3 body;

    public GameObject shieldAura;
    public GameObject vfx_explosion;
    public GameObject vfx_ripple;
    public Transform[] bulletPos2;
    public Transform[] bulletPos5;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;

    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 
    private bool _canShoot = true;

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        shieldAura.SetActive(false);
        vfx_ripple.SetActive(true);
        _isBoosted = false;

        transform.DOMoveY(-5, 1f).OnComplete(() => {
            GameManager_V3.Ins.canInteract = true;
            GameManager_V3.Ins.ShowTut();
        });
    }

    public void GetBooster()
    {
        boostedSound.Play();
        _isBoosted = true;
        spawnBulletDuration = 0.1f;
        Constant.BULLET_SPEED *= 1.2f;
        //Constant.BULLET_DAMAGE *= 2;
    }

    public void StartShooting()
    {
        movement.canMove = true;
        shieldAura.SetActive(true);
        vfx_ripple.SetActive(false);
        StartCoroutine(IE_SpawnBullet());
    }

    public void WaitToWin()
    {
        movement.canMove = false;
        _canShoot = false;
        Invoke(nameof(MoveWinGame), 1f);
        GameManager_V3.Ins.WaitToShowEndCard(2f);
    }

    void MoveWinGame()
    {
        transform.DOMove(new Vector3(0, 15f, 0), 1.5f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constant.ENEMY_TAG))
        {
            crashSound.Play();
            Instantiate(vfx_explosion, transform.position, Quaternion.identity);

            GameManager_V3.Ins.endGame = true;
            GameManager_V3.Ins.EndGame();
            gameObject.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }

    IEnumerator IE_SpawnBullet()
    {
        yield return new WaitForSeconds(spawnBulletDuration);

        if (!_isBoosted)
        {
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[0].position, Quaternion.identity);
            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletPos2[1].position, Quaternion.identity);
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
