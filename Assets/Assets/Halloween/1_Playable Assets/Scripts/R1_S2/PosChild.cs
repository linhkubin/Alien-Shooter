using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosChild : MonoBehaviour
{
    public int idPos;
    public bool isEmpty;
    public Transform tf_PosChild;
    public bool isFirstConfig;
    public StackShip stackShip;
    private void Start()
    {
        isEmpty = true;

        tf_PosChild = this.transform;
    }

    private void Update()
    {
        Check_Ship_Deactive();
    }
    public void Check_Ship_Deactive()
    {
        if (stackShip != null)
        {
            if (Vector3.Distance(tf_PosChild.position, stackShip.tf_This_Stack_Ship.position) > 0.5f)
            {
                stackShip = null;
            }else if (stackShip.obj_Ship.activeSelf == false)
            {
                stackShip = null;
            }
            

        }
        
    }
}
