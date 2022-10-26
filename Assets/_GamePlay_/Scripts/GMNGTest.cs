using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMNGTest : MonoBehaviour
{
    public static GMNGTest Ins;
    public GameObject prefabs;
    public GameObject prefabs2;

    public Transform posSpawn1;
    public Transform posSpawn2;
    private void Awake()
    {
        Ins = this;
    }
    private void Update()
    {
        spawnBullet();
    }
    public void spawnBullet()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefabs, posSpawn1.position, posSpawn1.transform.rotation);
            Instantiate(prefabs2, posSpawn2.position, posSpawn2.transform.rotation);
        }
        
    }
}
