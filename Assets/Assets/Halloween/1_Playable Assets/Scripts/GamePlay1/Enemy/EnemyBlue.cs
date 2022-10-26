using UnityEngine;
using DG.Tweening;

public class EnemyBlue : SpaceShip
{
    public Transform pos;
    public bool isLeft;
    public bool canMove = false;

    private Vector3 initPosition;
    private Vector3 startPos;

    private float speed = 20f;
    private float moveDuration = 1f;
    private bool reachedStartPos = false;

    void Start()
    {
        initPosition = transform.position;
        health = 2f;
        //moveDuration = Random.Range(1.5f, 3f);

        if (isLeft) startPos = EnemyManager.Ins.leftPos.position;
		else startPos = EnemyManager.Ins.rightPos.position;
    }

    void Update()
    {
        if (canMove)
		{
            if (!reachedStartPos)
		    {
                float distance = Vector3.Distance(transform.position, startPos);
                if (distance > 0.25f)
			    {
                    transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

                }
                else
			    {
                    reachedStartPos = true;
                    MoveToPos();
                }
		    }
        }
    }

    void MoveToPos()
    {
        transform.DOMove(pos.position, moveDuration);
    }

    public void MoveToInitPos()
    {
        transform.DOMove(initPosition, 2f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.BULLET_TAG))
        {
           
             TakeDamage(Constant.BULLET_DAMAGE);

        }
    }

    public void TakeDamaged(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_GS1.Ins.PlayExplosionSound();
            ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
