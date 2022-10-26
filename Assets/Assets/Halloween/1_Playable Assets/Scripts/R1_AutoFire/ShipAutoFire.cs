using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAutoFire : MonoBehaviour
{
    public static ShipAutoFire Ins;
    public float timingDelayLookTarget;
    public float maxTimingDelayLookTarget;
    private void Awake()
    {
        Ins = this;
    }

    [SerializeField] private float scanRadius = 3f;
    [SerializeField] private float fireDelay = 0.1f;
    [SerializeField] private LayerMask layers;
    [SerializeField] private Collider2D target;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePoint1;
    [SerializeField] private Transform firePoint2;
    [SerializeField] private Transform firePoint3;
    [SerializeField] private Transform firePoint4;
    [SerializeField] private Transform tf;
    private string fireBullet_Tag = "fireBullet";

    private bool canLookTarget = false;
    private void Start()
    {
        tf = transform;
        InvokeRepeating(fireBullet_Tag, 0f, fireDelay);
    }

    private void Update()
    {
        CheckEnvironment();

        timingDelayLookTarget += Time.deltaTime;

        if(timingDelayLookTarget >= maxTimingDelayLookTarget)
        {
            timingDelayLookTarget = 0;
            LookAtTarget();
        }
    }

    private void CheckEnvironment()
    {
        target = Physics2D.OverlapCircle(transform.position, scanRadius, layers);
        if(target != null)
        {

        }
    }
  
    public void LookAtTarget()
    {
        if(target != null)
        {
            Vector2 dir = target.transform.position - tf.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward * 2 * Time.deltaTime);
            //Quaternion lookRotation = Quaternion.LookRotation(dir);
            //Vector3 rotation = Quaternion.Lerp(tf.rotation, lookRotation, Time.deltaTime * 90).eulerAngles;
            //tf.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }
    }

    private void fireBullet()
    {
        if (target != null)
        {
            ObjectPooling.Ins.SpawnFromPool(Constant.BULLET_TAG, firePoint.position, firePoint.rotation);
            ObjectPooling.Ins.SpawnFromPool(Constant.BULLET_TAG, firePoint1.position, firePoint1.rotation);
            ObjectPooling.Ins.SpawnFromPool(Constant.BULLET_TAG, firePoint2.position, firePoint2.rotation);
            ObjectPooling.Ins.SpawnFromPool(Constant.BULLET_TAG, firePoint3.position, firePoint3.rotation);
            ObjectPooling.Ins.SpawnFromPool(Constant.BULLET_TAG, firePoint4.position, firePoint4.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }
}
