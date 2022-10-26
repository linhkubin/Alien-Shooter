using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDameRocket : MonoBehaviour
{
    public FighterMovement fighter;
    public PlayerR1BLH PlayerR1;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(Constant.ENEMY_ROCKET_TAG))
        {
            //other.gameObject.SetActive(false);
            //PlayerR1.health = 0;
        }
    }
}
