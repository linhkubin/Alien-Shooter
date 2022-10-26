using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletHomming : MonoBehaviour
{
    public AnimationCurve positionCurve, noiseCurve;
    
    public float speed;
    public float magnitudeRange;

    public Vector2 startPoint;
    public Vector2 targetPoint;
    private Vector2 horizontalVector;
    private float noisePosition;
    public float time;
    private float magnitude;
    public float damage;

    [HideInInspector]
    public bool isBoosted = false;
    [HideInInspector]
    public Transform boostedPos;

    private bool onActive = true;
    private bool moveToInit = false;

    //void OnEnable()
    //{
    //    onActive = true;
    //}

    //void OnDisable()
    //{
    //    onActive = false;
    //    isBoosted = false;
    //    moveToInit = false;
    //}

    //public void Start()
    //{
    //    //time = 0f;
    //    //startPoint = transform.position;

    //    targetPoint = startPoint + new Vector2(0, 20f);

    //    Vector2 direction = targetPoint - startPoint;
    //    horizontalVector = Vector2.Perpendicular(direction);
    //    magnitude = magnitudeRange;
    //}
    public void FixedUpdate()
    {
        CalculateAnimationOfBullets();
    }

    private void Update()
    {
        if (onActive)
        {
            //if (!isBoosted)
            //{
            //    transform.Translate(Vector3.up * Constant.BULLET_SPEED * Time.deltaTime);

            if (transform.position.y > 13f)
            {
                time = 0;
                ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
            //onActive = false;
            }
            //}
            //else
            //{
            //    if (!moveToInit)
            //    {
            //        moveToInit = true;
            //        transform.localScale = Vector3.one * 0.75f;
            //        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
            //        transform.DOMove(boostedPos.position, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
            //        {
            //            isBoosted = false;
            //        });
            //    }
            //}
        }
    }

    public void setStartPos(Transform posStart)
    {
        time = 0f;
        startPoint = posStart.position;

        targetPoint = startPoint + new Vector2(0, 20f);

        Vector2 direction = targetPoint - startPoint;
        horizontalVector = Vector2.Perpendicular(direction);
        magnitude = magnitudeRange;
    }

    //public void Settarget(Vector2 target) {
    //    targetPoint = target;
    //    time = 0;
    //}

    public void turnOnBullet()
    {
        this.gameObject.SetActive(true);
    }

    public void CalculateAnimationOfBullets()
    {
        if(startPoint == null)
        {
            return;
        }

        if (time <= positionCurve.keys[positionCurve.length - 1].time)
        {
            noisePosition = noiseCurve.Evaluate(time);
            transform.position = Vector3.Lerp(startPoint, targetPoint, positionCurve.Evaluate(time)) +
                new Vector3(noisePosition * horizontalVector.x * magnitude, noisePosition * horizontalVector.y * magnitude);
            time += Time.deltaTime * speed;


            Vector2 nextPoint = Vector3.Lerp(startPoint, targetPoint, positionCurve.Evaluate(time)) +
                new Vector3(noiseCurve.Evaluate(time) * horizontalVector.x * magnitude, noiseCurve.Evaluate(time) * horizontalVector.y * magnitude);

            Vector2 direction = (nextPoint - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle - 90);
        }
        else if (time >= 1)
        {
            this.gameObject.SetActive(false);
            time = 0;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.ENEMY_TAG))
        {
            time = 0;
            ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
            ObjectPooling.Ins.SpawnFromPool(Constant.SPARK_TAG, transform.position, Quaternion.identity);
            
        }
    }
}
