using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    #region Singleton
    public static Boss Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public AudioSource lazerSound;
    public AudioSource shootSound;

    public float healthMax;
    private float health;

    public Animator anim;
    public Animator lazerAnim;
    private int animState = 0;
    public Image healthBar;

    public GameObject boosterItem;

    private int boosterNum = 1;
    private bool canSpawn = true;
    private int nextSpawn = 10;

    void Start()
    {
        lazerAnim.gameObject.SetActive(false);

        health = healthMax;
        healthBar.fillAmount = healthMax / 25f;

        transform.DOMoveY(6.5f, 1f);
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
            DestroyBoss();
        }
        else
        {
            if (boosterNum > 0 && canSpawn && health < healthMax && health % nextSpawn == 0)
            {
                canSpawn = false;
                boosterNum--;
                SpawnBooster();
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
        StartCoroutine(IE_StartSkill());
	}

    public void Skill1()
    {
        SetAnim(1);
        StartCoroutine(IE_Skill1(0.7f));
    }

    public void Skill2()
    {
        transform.DOMoveY(1, 1f).OnComplete(() =>
        {
            StartCoroutine(IE_Skill2(0.6f, 4));
        });
    }

    public void EndSkill2()
    {
        ResetAnim();
        transform.DOMoveY(6.5f, 1f);
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

        EndCard_R1P.Ins.WaitToShowEndCard(3f, true);

        Destroy(gameObject);
    }

    public void Shooting()
    {
        shootSound.PlayOneShot(shootSound.clip);
        BulletEnemy_R1P bullet = ObjectPooling.Ins.SpawnFromPool(Constant.ENEMY_BULLET_TAG, transform.position, Quaternion.identity).GetComponent<BulletEnemy_R1P>();

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

        yield return new WaitForSeconds(1.2f);
        lazerAnim.gameObject.SetActive(false);
        ResetAnim();
    }

    IEnumerator IE_Skill2(float waitFrameTime, int bulletNum)
    {
        SetAnim(2);
        yield return new WaitForSeconds(waitFrameTime);
        int time = 0;
        while(time < bulletNum)
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
