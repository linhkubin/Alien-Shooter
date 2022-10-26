using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    public List<GameObject> BarrierList = new List<GameObject>();

    public float timeCounter;
    public float maxTimeCounter;

    public int countBarrier = 0;

    public bool canSpawn = true;
    void Start()
    {
        Instantiate(BarrierList[3], this.transform.position, Quaternion.identity);
    }


    void Update()
    {
        if(StackShipLogic.Ins.isDeadAll == false)
        {
            SpawnBarrier();
        }
        
    }

    public void SpawnBarrier()
    {
        if (countBarrier >= 3)
        {
            return;
        }

        timeCounter += Time.deltaTime;

        if (timeCounter >= maxTimeCounter)
        {
            timeCounter = 0;
            Instantiate(BarrierList[countBarrier], this.transform.position, Quaternion.identity);
            countBarrier++;
        }
    }

    
}
