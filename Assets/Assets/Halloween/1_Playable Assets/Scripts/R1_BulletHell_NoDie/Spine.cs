using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    public float time;
    public float speedSpine;

    [Range(-1, 1)]
    public int leftOrRight;

    // Update is called once per frame
    void Update()
    {
        if (leftOrRight > 0)
        {
            time += Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(0, 0, time * speedSpine);
        }
        else if (leftOrRight < 0)
        {
            time += Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(0, 0, time * speedSpine * -1);
        }
        else if (leftOrRight == 0)
        {
            time += Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(0, 0, time * speedSpine * 0);
        }
    }
}
