using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBulletHell : MonoBehaviour
{
    [SerializeField]
    private int bulletsAmount = 10;

    [SerializeField]
    private float startAngle = 90f, endAngle = 270f;

    private Vector2 bulletMoveDirection;

    public float timeCount;
    public float maxTimeCount;
    public GameObject local;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    
    private void FixedUpdate()
    {
        timeCount += Time.fixedDeltaTime;
        
        if(timeCount >= maxTimeCount)
        {
            timeCount = 0;
            Fire();
        }
        //transform.Rotate(new Vector3(0, 0, 6));

        //return;
        
    }

    private void Fire()
    {
        float angleStep = (endAngle - startAngle) / bulletsAmount;

        float angle = startAngle;

        for(int i = 0; i < bulletsAmount; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;

            bul.transform.SetParent(local.transform);
            bul.SetActive(true);
            //bul.GetComponent<BulletScript>().SetMoveDirection(bulDir);

            angle += angleStep;
        }
    }
}
