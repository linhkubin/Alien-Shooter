using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour
{
    public float speed;
    public float count  ;

    public Transform bullet;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed));

        if (Time.frameCount % count == 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
