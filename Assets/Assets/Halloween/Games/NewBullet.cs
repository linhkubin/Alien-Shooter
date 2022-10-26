using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBullet : MonoBehaviour
{
    public float speed;

    //// Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        //transform.position = Vector3.MoveTowards(transform.position, Vector3.up * Time.deltaTime * speed, 1);
    }

}
