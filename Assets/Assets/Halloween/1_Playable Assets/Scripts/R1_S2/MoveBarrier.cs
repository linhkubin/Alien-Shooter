using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveBarrier : MonoBehaviour
{
    private void Update()
    {
        MoveToPos();

        if(StackShipLogic.Ins.isDeadAll == true)
        {
            Destroy(gameObject);
        }
    }
    public void MoveToPos()
    {
        transform.Translate(Vector3.down * 3.5f * Time.deltaTime);

        if(this.transform.position.y <= -16f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
