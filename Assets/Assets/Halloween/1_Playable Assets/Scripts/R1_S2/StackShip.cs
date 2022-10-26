using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Luna.Unity;

public class StackShip : MonoBehaviour
{
    public int id = 0;

    public Animator anim;

    public float health;

    public Transform tf_This_Stack_Ship;

    public bool _CanShoot = true;

    private float spawnBulletDuration;

    public Transform bulletSpawnPos;

    private ObjectPooling objectPooling;

    public bool hasSpawn = false;

    public int countShip = 0;

    public AudioSource SourceSound;

    public Transform Tf_Ship;
    public GameObject obj_Ship;
    //
    public Transform tf_Parent;
    public Transform tfPos_Target;
    public bool isFly_First = true;

    public bool hasFinishMove = false;
    
    public bool isDead = false;
    [LunaPlaygroundFieldStep(0.1f)]
    [LunaPlaygroundField("SpeedMove", 1, "Game Settings")]
    [Range(10, 100)]
    public float speed_Start;

    public PosChild posChild;
    public bool isMoving_Order;
    public bool isOder;
    //
    public bool isCheck;
    public int index_PosChild;
    private void OnEnable()
    {
        isMoving_Order = false;
        isOder = false;
    }
    void Start()
    {
        obj_Ship = gameObject;
        tf_This_Stack_Ship = transform;
        isMoving_Order = false;
        //speed_Start = 25;
        StackShipLogic.Ins.addShip(this);

        objectPooling = ObjectPooling.Ins;

        anim.SetFloat("Speed", Random.Range(0.8f, 1.2f));

        spawnBulletDuration = Random.Range(Random.Range(0.2f, 0.3f), 0.5f);
        //spawnBulletDuration = 1;

        Tf_Ship = this.transform;
    }

   
    private void Update()
    {
        if (this.isDead == true)
        {
            StackShipLogic.Ins.listShipUp.Remove(this);
            //StackShipLogic.Ins.listPosChild[index_PosChild].stackShip = null;
        }

        if (isFly_First )
        {
            if (tfPos_Target != null)
            {
                Vector3 dir = tfPos_Target.position - Tf_Ship.position;
                Tf_Ship.Translate(dir.normalized * Time.deltaTime * speed_Start);
                if (Vector3.Distance(Tf_Ship.position, tfPos_Target.position) < 0.1f)
                {
                    isFly_First = false;
                    isOder = true;
                    Tf_Ship.SetParent(tf_Parent);

                    posChild = StackShipLogic.Ins.listPosChild[index_PosChild];
                    StackShipLogic.Ins.listPosChild[index_PosChild].stackShip = this;


                    StackShipLogic.Ins.int_Count_may_bay++;
                }
            }
            
        }
        if (isOder && StackShipLogic.Ins.int_Count_may_bay > 3)
        {
            //Set_check_emptyBeforePos();
        }



        Check_Pos();
    }


    public void Check_Pos()
    {
        if (posChild != null)
        {
            if (Vector3.Distance(posChild.tf_PosChild.position,tf_This_Stack_Ship.position) > 0.5f)
            {
                for (int i = 0; i < StackShipLogic.Ins.listPosChild.Count; i++)
                {
                    if (Vector3.Distance(StackShipLogic.Ins.listPosChild[i].tf_PosChild.position, tf_This_Stack_Ship.position) < 0.2f)
                    {
                        posChild = StackShipLogic.Ins.listPosChild[i];
                    }
                }
            }
        }

    }





    private void FixedUpdate()
    {
        if (hasSpawn == false && GameManager_R1S2.Ins.startGame == true)
        {
            hasSpawn = true;
            SpawnBullet();
        }
    }

    public void Set_Tf_Parent_Target(Transform _tf_Parent, Transform _tf_Target, int index)
    {
        tf_Parent = _tf_Parent;
        tfPos_Target = _tf_Target;
        index_PosChild = index;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_BULLET_TAG) || collision.CompareTag(Constant.ENEMY_TAG) || collision.CompareTag(Constant.BOSS_BULLET_TAG))
        {
            TakeDamage(Constant.BULLET_DAMAGE_PLAYER);

            StackShipLogic.Ins.listShipUp.Remove(collision.GetComponent<StackShip>());

            tfPos_Target.GetComponent<PosChild>().isEmpty = true;
        }
    }

    

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            EnemyManager_R1S2.Ins.PlayExplosionSound();
            
            DestroyShip();
        }
    }

    public void DestroyShip()
    {
        ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity);
        //StackShipLogic.Ins.listPosChild[index_PosChild].stackShip = null;
        StackShipLogic.Ins.listShipUp.Remove(this);
        isDead = true;
        gameObject.SetActive(false);
        //posChild.stackShip = null;
        
    }

    public void SpawnBullet()
    {
        StartCoroutine(IE_SpawnBullet());
    }

    IEnumerator IE_SpawnBullet()
    {
        if (GameManager_R1S2.Ins.TurnOffBullet == false)
        {
            yield return new WaitForSeconds(spawnBulletDuration);

            objectPooling.SpawnFromPool(Constant.BULLET_TAG, bulletSpawnPos.position, Quaternion.identity);

            if (SourceSound != null)
            {
                SourceSound.Play();
            }
            if (_CanShoot) StartCoroutine(IE_SpawnBullet());
        }
    }

}

//public void set_check_emptyBeforePos(int indexBefore)
//{

//        if (StackShipLogic.Ins.listPosChild[indexBefore].stackShip == null)
//        {
//            shipTf.DOMove(StackShipLogic.Ins.listPosChild[indexBefore].tf_PosChild.position, 0.2f);
//            StackShipLogic.Ins.listPosChild[posChild.idPos].stackShip = null;

//            StackShipLogic.Ins.listPosChild[indexBefore].stackShip = this;

//        }
//}

/*
 
2s check 1 lần
    Nếu die
        
        0 1 2 3




public void Set_check_emptyBeforePos()
    {
        if (posChild.idPos > 0)
        {
            int indexBefore = posChild.idPos - 1;
            if (StackShipLogic.Ins.listPosChild[indexBefore].stackShip != null)
            {
                Debug.Log(StackShipLogic.Ins.listPosChild[indexBefore].stackShip.gameObject.activeSelf);


                Debug.Log(!isMoving_Order);


                if (StackShipLogic.Ins.listPosChild[indexBefore] != null && !isMoving_Order)
                {
                    isMoving_Order = true;

                    Tf_Ship.DOMove(StackShipLogic.Ins.listPosChild[indexBefore].tf_PosChild.position, 0.2f).OnComplete(() => {
                        isMoving_Order = false;
                    });
                    StackShipLogic.Ins.listPosChild[posChild.idPos].stackShip = null;

                    StackShipLogic.Ins.listPosChild[indexBefore].stackShip = this;

                }


            }
            
        }

    }

 * 
 */