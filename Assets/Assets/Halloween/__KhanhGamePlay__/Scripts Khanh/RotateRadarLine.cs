using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRadarLine : MonoBehaviour
{
    public Transform tf_Sweep;

    private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360f;
    }

    // Update is called once per frame
    void Update()
    {
        RotateRadar();
    }
    
    public void RotateRadar()
    {
        tf_Sweep.localEulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }
}
