using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BossR1BulletHell : MonoBehaviour
{
    #region Singleton
    public static BossR1BulletHell Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource lazerSound;
    public AudioSource shootSound;

    public float healthMax;
    public float health;

    public Animator anim;
    public Animator lazerAnim;
    private int animState = 0;
    public Image healthBar;
    public AudioSource BossDie;
    public GameObject boosterItem;

    private int boosterNum = 1;
    private bool canSpawn = true;
    private int nextSpawn = 10;
    public Transform boss;

    public Transform spawnBulletPos;

    public float timingSpineBulletHell;
    public float maxTimingSpineBulletHell; 
    public float timingSpineBulletHell2;
    public float maxTimingSpineBulletHell2; 
    public float timingRocket;
    public float maxTimingRocket;

    public GameObject spineBulletHell;
    public AudioSource laserSpineGun;

    public int bulletHellFireCount = 0;

    public GameObject GoToTheHell;

    void Start()
    {
        lazerAnim.gameObject.SetActive(false);

        health = healthMax;
        healthBar.fillAmount = healthMax / 25f;

    }

    private void Update()
    {
        //if (GameManagerR1BLH.Ins.bossIsDie == false && GameManagerR1BLH.Ins.isLose == false)
        //{
        //    if (GameManagerR1BLH.Ins.isClick == true)
        //    {
        //        timingSpineBulletHell += Time.deltaTime;

        //        if (timingSpineBulletHell >= maxTimingSpineBulletHell && bulletHellFireCount <= 1)
        //        {
        //            spineBulletHell.SetActive(true);

        //            if (timingSpineBulletHell >= 2.5)
        //            {
        //                spineBulletHell.SetActive(false);
        //                timingSpineBulletHell2 += Time.deltaTime;

        //                if (timingSpineBulletHell2 >= maxTimingSpineBulletHell2)
        //                {
        //                    timingSpineBulletHell = 0;
        //                    timingSpineBulletHell2 = 0;
        //                    bulletHellFireCount++;
        //                }

        //            }

        //        }

        //        if (bulletHellFireCount >= 2)
        //        {
        //            timingRocket += Time.deltaTime;

        //            if (timingRocket >= maxTimingRocket)
        //            {
        //                GameManagerR1BLH.Ins.homingLauncherSound.Play();

        //                GoToTheHell.SetActive(true);

        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    boss.transform.DOMoveY(-1.5f, 1.5f);

        //    spineBulletHell.SetActive(false);
        //}

        //if (health <= 420)
        //{
        //    timingRocket += Time.deltaTime;

        //    if (timingRocket >= maxTimingRocket)
        //    {
        //        GoToTheHell.SetActive(true);
        //    }
        //}

        if (GameManagerR1BLH.Ins.canMove == true)
        {
            WaitToShowBoss(0.25f);
            
        }
    }

        //public void OnTriggerEnter2D(Collider2D collision)
        //   {
        //       if (collision.CompareTag(Constant.BULLET_TAG))
        //       {
        //           TakeDamage(Constant.BULLET_DAMAGE);
        //       }
        //   }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health < 0)
        {
            GameManagerR1BLH.Ins.endGame = true;
            GameManagerR1BLH.Ins.canPlay = false;
            DestroyBoss();
            GameManagerR1BLH.Ins.bossIsDie = true;
        }
        else
        {
            if (boosterNum > 0 && canSpawn && health < healthMax && health % nextSpawn == 0)
            {
                canSpawn = false;
                boosterNum--;
                //SpawnBooster();
                nextSpawn += nextSpawn;
                Invoke(nameof(ResetBooster), 0.25f);
            }

            healthBar.fillAmount = health / healthMax;
        }
    }

    public void SpawnBooster()
    {
        Transform t = Instantiate(boosterItem, transform.position, Quaternion.identity).transform;
        t.DOJump(new Vector3(Random.Range(t.transform.position.x - 2, t.transform.position.x + 2), t.transform.position.y - Random.Range(1, 3), 0),
            Random.Range(1, 2), 1, Random.Range(0.5f, 1f)).SetEase(Ease.Linear);
    }

    public void StartSkill()
    {
        if(GameManagerR1BLH.Ins.endGame == false)
        {
            StartCoroutine(IE_StartSkill());
        }
    }

    public void Skill1()
    {
        if (GameManagerR1BLH.Ins.isLose == false)
        {
            SetAnim(1);
            StartCoroutine(IE_Skill1(0.7f));
        }
        else
        {
            return;
        }
    }

    public void Skill2()
    {
        if(GameManagerR1BLH.Ins.isLose == false)
        {
            boss.transform.DOMoveY(1, 0.3f).OnComplete(() =>
            {
                StartCoroutine(IE_Skill2(0.6f, 4));
            });
        }
        else
        {
            return;
        }
    }


    public void EndSkill2()
    {
        ResetAnim();
        transform.DOMoveY(5f, 1f);
    }

    private void ResetBooster()
    {
        canSpawn = true;
    }

    private void SetAnim(int action)
    {
        if (animState != action)
        {
            animState = action;
            anim.SetInteger("Action", animState);
        }
    }

    private void ResetAnim()
    {
        animState = 0;
        anim.SetInteger("Action", animState);
    }

    public void DestroyBoss()
    {
        Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
        t.localScale = Vector3.one * 5f;
        //EndCard_R1P.Ins.WaitToShowEndCard(3f, true);
        Destroy(gameObject);
    }

    public void Shooting()
    {
        shootSound.PlayOneShot(shootSound.clip);
        BulletEnemy_R1P bullet = ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_BULLET_TAG, spawnBulletPos.position, Quaternion.identity).GetComponent<BulletEnemy_R1P>();

        Vector3 targetPos = new Vector3(Random.Range(-4, 4), -14f, 0);
        bullet.targetPos = targetPos;
        bullet.MoveToPos();
    }

    IEnumerator IE_StartSkill()
    {
        yield return new WaitForSeconds(Random.Range(1, 2f));
        Skill1();

        yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        Skill2();
     
        yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        StartCoroutine(IE_StartSkill());

    }

    IEnumerator IE_Skill1(float waitFrameTime)
    {
        yield return new WaitForSeconds(waitFrameTime);
        lazerAnim.gameObject.SetActive(true);
        lazerSound.Play();
        lazerSound.volume = 10;

        yield return new WaitForSeconds(1.2f);
        lazerAnim.gameObject.SetActive(false);
        ResetAnim();
    }

    IEnumerator IE_Skill2(float waitFrameTime, int bulletNum)
    {
        SetAnim(2);
        yield return new WaitForSeconds(waitFrameTime);
        int time = 0;
        while (time < bulletNum)
        {
            time++;
            float randWait = Random.Range(0.1f, 0.25f);
            yield return new WaitForSeconds(randWait);

            Shooting();
        }
        yield return new WaitForSeconds(1f);
        EndSkill2();
    }
    


    public void WaitToShowBoss(float time)
    {
        Invoke(nameof(ShowBoss), time);
    }

    private void ShowBoss()
    {
        boss.DOLocalMoveY(5f, 1).SetEase(Ease.Linear);
    }
}
