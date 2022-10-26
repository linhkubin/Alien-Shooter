using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentEnemyMove : MonoBehaviour
{
    private Transform thisPos;

    private float speed;
    private void Start()
    {
        thisPos = this.transform;
        

        speed = Random.Range(7, 9);
    }
    void Update()
    {
        MoveTheParent();
        if (GameManager_R1S2.Ins.isLose == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void MoveTheParent()
    {
        transform.position = Vector3.MoveTowards(thisPos.position, GameManager_R1S2.Ins.player.gameObject.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(this.transform.position, GameManager_R1S2.Ins.player.gameObject.transform.position) <= 5f)
        {
            foreach (var item in thisPos.GetComponentsInChildren<EnemyMove>())
            {
                if(item != null)
                {
                    item.gameObject.transform.SetParent(null);

                    item.canMoving = true;

                }

            }
        }
    }
}
