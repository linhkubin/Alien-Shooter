using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Fighter : MonoBehaviour
{
    #region Singleton
    public static Fighter Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public FighterMovement movement;

    public GameObject[] ships;
    [HideInInspector] public int shipIdx = 0;

    public GameObject shieldAura;
    public GameObject vfx_explosion;
    public Transform[] bulletPos2;
    public Transform[] bulletPos5;

    //-----------Sound------------
    public AudioSource crashSound;
    public AudioSource boostedSound;
    public AudioSource shootSound;

    private float spawnBulletDuration = 0.2f;

    private ObjectPooling objectPooling;

    private bool _isBoosted = false; //Kiem tra xem co dang an booster ko 

    void Start()
    {
        objectPooling = ObjectPooling.Ins;
        shieldAura.SetActive(false);
        _isBoosted = false;

        //transform.DOMoveY(3, 1f);
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
        shieldAura.SetActive(true);
        ships[shipIdx].transform.DOScale(Vector3.one, 1f);

        transform.DOMoveY(-7, 1f).OnComplete(()=> {
            movement.canMove = true;

            for (int i = 0; i < ships.Length; i++)
            {
                if (i != shipIdx) Destroy(ships[i]);
            }

            StartCoroutine(IE_SpawnBullet());
        });
    }

    public void WaitToWin()
	{
        movement.canMove = false;
        Invoke(nameof(MoveWinGame), 1f);
        //GameManager.Ins.EndGame();
        GameManager.Ins.WaitToShowEndCard(2f);
    }

    void MoveWinGame()
	{
        transform.DOMove(new Vector3(0, 15f, 0), 1.5f).OnComplete(() => {
            //GameManager.Ins.ShowEndCard();
            gameObject.SetActive(false);
        });
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag(Constant.ENEMY_TAG))
		{
            //Endgame
            ships[shipIdx].SetActive(false);
            Instantiate(vfx_explosion, transform.position, Quaternion.identity);
            crashSound.Play();

            GameManager.Ins.endGame = true;
            GameManager.Ins.EndGame();
            Destroy(gameObject);
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
        StartCoroutine(IE_SpawnBullet());
    }
}
