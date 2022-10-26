using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_V1 : MonoBehaviour
{
    public Rigidbody rb;

    private Vector2 playerDir;
    private float timeStamp;
    private bool moveToPlayer;
    private Player_R1_RM_2 fighter;
    private Transform fighterTf;

    float timingAutoBoost;
    float maxTimingAutoBoost = 0.4f;

    void Start()
    {
        //if (GameManager_R1.Ins.isLose == false)
        //{
            fighter = Player_R1_RM_2.Ins;
            //    fighter = Player_R1_RM_2.Ins;

            fighterTf = fighter.gameObject.transform;
        //}
        //else
        //{
        //    return;
        //}
    }

    void Update()
    {
        //if (GameManager_GS1.Ins.isLose == false)
        //{
        //if (fighter.isDead == true)
        //{
            //this.gameObject.SetActive(false);
        //}

        if (moveToPlayer)
        {
            playerDir = -(transform.position - fighterTf.position).normalized;
            rb.velocity = new Vector2(playerDir.x, playerDir.y) * 30f * (Time.time / timeStamp);
        }
        else
        {
            transform.Translate(Vector2.down * 2.7f * Time.deltaTime);
        }

        timingAutoBoost += Time.deltaTime;
        if (timingAutoBoost >= maxTimingAutoBoost)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, fighter.transform.position, 30f * Time.deltaTime);
        }

        if (Vector3.Distance(this.transform.position, fighterTf.position) <= 0.1f)
        {
            this.gameObject.SetActive(false);

        }
        //}
        //else
        //{
        //    this.gameObject.SetActive(false);
        //}
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.name.Equals("Magnet"))
        {
            timeStamp = Time.time;
            moveToPlayer = true;
        }
    }
}
