using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Boss_R1S2 : MonoBehaviour
{
    #region Singleton
    public static Boss_R1S2 Ins;
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
    public Transform boss;
    public Transform spawnBulletPos;

    public GameObject GoToTheHell;

    private bool comePos = false;

    public float timingRocket;
    public float maxTimingRocket;
    void Start()
    {
        lazerAnim.gameObject.SetActive(false);

        health = healthMax;
        healthBar.fillAmount = healthMax / 25f;
    }

    private void Update()
    {
        //if (health <= 650)
        //{
        //    timingRocket += Time.deltaTime;

        //    if (timingRocket >= maxTimingRocket)
        //    {
        //        GoToTheHell.SetActive(true);
        //    }
        //}

        if(GameManager_R1S2.Ins.isLose == true)
        {
            
        }

        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
            
            TakeDamage(Constant.BULLET_DAMAGE);

            if (StackShipLogic.Ins.listShipUp.Count <= 7 && health <= 700)
            {
                TakeDamage(25);
            }
        }
    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.fillAmount = health / healthMax;
        if (health < 0)
        {
            DestroyBoss();
        }
    }

    public void DestroyBoss()
    {
        Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
        t.localScale = Vector3.one * 5f;
        Destroy(gameObject);

        StackShipLogic.Ins.WaitToTurnOffBullet(0.25f);
    }

    public void StartSkill()
    {
        StartCoroutine(IE_StartSkill());
        
    }

    public void Skill1()
    {
       
        SetAnim(1);
        StartCoroutine(IE_Skill1(0.7f));
        
    }

    public void Skill2()
    {
        boss.transform.DOMoveY(1, 0.3f).OnComplete(() =>
        {
            StartCoroutine(IE_Skill2(0.6f, 4));
        });
    }

    public void EndSkill2()
    {
        ResetAnim();
        transform.DOMoveY(5f, 1f);
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

    public void Shooting()
    {
        shootSound.PlayOneShot(shootSound.clip);
        BulletEnemy_R1P bullet = ObjectPooling.Ins.SpawnFromPool(Constant.BOSS_BULLET_TAG, spawnBulletPos.position, Quaternion.identity).GetComponent<BulletEnemy_R1P>();
        Vector3 targetPos = new Vector3(Random.Range(-4, 4), -14f, 0);
        bullet.targetPos = targetPos;
        bullet.MoveToPos();
    }

    IEnumerator IE_StartSkill()
    {
        if(GameManager_R1S2.Ins.isLose == false)
        {
            yield return new WaitForSeconds(Random.Range(1, 2f));
            Skill1();

            yield return new WaitForSeconds(Random.Range(2.5f, 4f));
            Skill2();

            yield return new WaitForSeconds(Random.Range(2.5f, 4f));
            StartCoroutine(IE_StartSkill());
        }
    }

    IEnumerator IE_Skill1(float waitFrameTime)
    {
        if (GameManager_R1S2.Ins.isLose == false)
        {
            yield return new WaitForSeconds(waitFrameTime);
            lazerAnim.gameObject.SetActive(true);
            lazerSound.Play();
            lazerSound.volume = 10;

            yield return new WaitForSeconds(1.2f);
            lazerAnim.gameObject.SetActive(false);
            ResetAnim();
        }
    }

    IEnumerator IE_Skill2(float waitFrameTime, int bulletNum)
    {
        if (GameManager_R1S2.Ins.isLose == false)
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
    }

    public void WaitToShowBoss(float time)
    {

        Invoke(nameof(ShowBoss), time);
    }

    private void ShowBoss()
    {
        GameManager_R1S2.Ins.ShowWarning(false);
        
        boss.DOLocalMoveY(5f, 1.5f).SetEase(Ease.Linear).OnComplete(() => {

            Invoke(nameof(StartSkill),0.25f);
        });
    }
}
