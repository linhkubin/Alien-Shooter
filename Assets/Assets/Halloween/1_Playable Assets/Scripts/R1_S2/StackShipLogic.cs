using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StackShipLogic : MonoBehaviour
{
    public static StackShipLogic Ins;
    

    private Vector3 posSpawn;

    private int index = 1;

    public bool hasSpawn = false;

    public Transform Parent;

    public bool canCollision;

    public int countSpawn = 0;

    public bool isDeadAll = false;

    public List<StackShip> listCountShip = new List<StackShip>();
    public List<StackShip> listShipUp = new List<StackShip>();
    public List<PosChild> listPosChild = new List<PosChild>();


    public AudioSource shootingSound;

    public bool isActiveEnemy = false;

    public GameObject ShipEndcard;

    public GameObject EndcardBoardNumber;
    //
    public int int_Count_may_bay;
    public bool isfisttttt;
    private void Awake()
    {
        isfisttttt = false;
        int_Count_may_bay = 0;
        Ins = this;
    }
    private void Start()
    {
        posSpawn = new Vector3(Random.Range(-4, 4), -18);
        canCollision = true;
        transform.DOMoveY(-6, 0.5f).OnComplete(() => {
            GameManager_R1S2.Ins.canInteract = true;
            GameManager_R1S2.Ins.ShowTut();
        });
        InvokeRepeating("Check_List", 2f, 2f);
    }
    


    public void Check_List()
    {
        int count = 0;

        for (int i = 0; i < listPosChild.Count; i++)
        {
            if (listPosChild[i].stackShip != null)
            {
                listPosChild[i].stackShip.tf_This_Stack_Ship.position = listPosChild[count].tf_PosChild.position;

                //listPosChild[i].stackShip.tf_This_Stack_Ship.DOMove(listPosChild[count].tf_PosChild.position,0.1f);

                listPosChild[count].stackShip = listPosChild[i].stackShip;
                //listPosChild[i].stackShip = null;
                count++;
            }
        }

    }

    IEnumerator IE_Check()
    {
        yield return new WaitForSeconds(0.5f);
        Check_List();
    }
    //public void set_OrderPosChild()
    //{

    //    for (int i = listPosChild.Count - 2; i > 0; i--)
    //    {
    //        if (listPosChild[i].stackShip == null)
    //        {
    //            listPosChild[i + 1].stackShip.set_check_emptyBeforePos(i);
    //        }
    //    }
    //}

    public void ShipMoveToPos()
    {
        if (listCountShip.Count >= 25)
        {
            return;
        }

        
       
        StackShip ship = ObjectPooling.Ins.SpawnFromPool(Constant.PLAYER_TAG, posSpawn, Quaternion.identity).GetComponent<StackShip>();
        ship.Set_Tf_Parent_Target(Parent, listPosChild[index].tf_PosChild, index);
        //ship.posChild = listPosChild[index];
        //listPosChild[index].stackShip = ship;
       
        index++;

        listCountShip.Add(ship);

    }

  
    public void WaitToTurnOffBullet(float time)
    {   
        Invoke(nameof(TurnOffBullet), time);
    }

    public void TurnOffBullet()
    {
        GameManager_R1S2.Ins.TurnOffBullet = true;
        GameManager_R1S2.Ins.canMoving = false;
        this.GetComponent<Collider>().enabled = false;
        MoveEndGame();

    }

    public void MoveEndGame()
    {
        StartCoroutine(IE_MoveEndGame());
    }

    IEnumerator IE_MoveEndGame()
    {
        yield return new WaitForSeconds(1f);
        GameManager_R1S2.Ins.canMoving = false;
        transform.DOMove(new Vector3(0, -6, 0), 0.25f).OnComplete(() => 
        {
            transform.DOMove(new Vector3(0, 25, 0), 1.5f).OnComplete(() => {

            //WinCard.SetActive(true);

            EnemyManager_R1S2.Ins.SetupEnemyRound2();

            EndcardBoardNumber.SetActive(true);

            EndcardBoardNumber.transform.DOMove(new Vector3(0, 4.75f, 0), 1f);

            ShipEndcard.transform.DOMove(new Vector3(0, -6.5f, 0), 1f).OnComplete(() => {

                GameManager_R1S2.Ins.ShowEndCard();


                });
            });
        });
    }

    private void Update()
    {
        if(listShipUp.Count <= 0)
        {
            isDeadAll = true;

            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().detectCollisions = false;

            GameManager_R1S2.Ins.isLose = true;
        }
    }

    private void FixedUpdate()
    {
        PlayShootingSound();
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.PLUS1_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                Debug.Log("Spawn1");
                ShipMoveToPos();
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS2_BULLET_TAG))
        {
            if (canCollision)
            {
                Debug.Log("Spawn2");
                canCollision = false;
                ShipMoveToPos();
                ShipMoveToPos();
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS3_BULLET_TAG))
        {
            if (canCollision)
            {
                Debug.Log("Spawn3");
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS4_BULLET_TAG))
        {
            if (canCollision)
            {
                Debug.Log("Spawn3");
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS5_BULLET_TAG))
        {
            if (canCollision)
            {
                Debug.Log("Spawn5");
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                if (isActiveEnemy == false)
                {
                    isActiveEnemy = true;
                    SpawnEnemy.Ins.canSpawn = true;
                }
            }
        }
        else if (collision.CompareTag(Constant.PLUSX2_BULLET_TAG))
        {
            if (canCollision)
            {
                Debug.Log("Spawn2");
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
            }
        }
        else if (collision.CompareTag(Constant.PLUS10_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos(); 
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                ShipMoveToPos();
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);
                if(isActiveEnemy == false)
                {
                    isActiveEnemy = true;
                    SpawnEnemy.Ins.canSpawn = true;
                }
                
            }
        }
        else if (collision.CompareTag(Constant.MINUS10_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);

            }
        }
        else if (collision.CompareTag(Constant.MINUS5_BULLET_TAG))
        {
            if (canCollision)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (listShipUp.Count - 1 >= 0)
                    {
                        listShipUp[listShipUp.Count - 1].gameObject.SetActive(false);
                        listShipUp[listShipUp.Count - 1].gameObject.GetComponent<StackShip>().TakeDamage(999);
                    }
                    
                }

                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);

            }
        }
        else if (collision.CompareTag(Constant.MINUS66_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);

            }
        }
        else if (collision.CompareTag(Constant.X0_BULLET_TAG))
        {
            if (canCollision)
            {
                canCollision = false;
                collision.GetComponent<Collider>().enabled = false;
                collision.GetComponent<CheckParent>().Parent.SetActive(false);

            }
        }

        if (collision.CompareTag(Constant.RESET_BULLET_TAG))
        {
            canCollision = true;

            collision.GetComponent<Collider>().enabled = false;
        }
    }

    public void PlayShootingSound()
    {
        if(listShipUp.Count > 0)
        {
            for (int i = 0; i < listShipUp.Count; i++)
            {
                if(i < 4)
                {
                    listShipUp[i].SourceSound = shootingSound;
                }
            }
        }
        else
        {
            return;
        }
    }
    public void addShip(StackShip ship)
    {
        listShipUp.Add(ship);
    }
}
