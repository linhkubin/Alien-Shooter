using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneFollow : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    public Transform target;

    

    private void Update()
    {
       
            transform.position = Vector2.MoveTowards(transform.position, target.position, Random.Range(minSpeed, maxSpeed) * Time.deltaTime);

    }
}
