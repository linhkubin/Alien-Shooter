using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteIfLose : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameManager_R1S2.Ins.isLose == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
