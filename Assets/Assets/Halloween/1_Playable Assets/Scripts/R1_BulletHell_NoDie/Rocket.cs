using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rocket : MonoBehaviour
{
    public Transform Target;

    public Player_R1S2 ParentTarget;

    public Rigidbody2D rb;

    public float speed = 5f;

    public float rotateSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)Target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;

        if (Vector3.Distance(this.transform.position, Target.position) <= 3f)
        {
            gameObject.SetActive(false);
            Target.gameObject.SetActive(false);
            ParentTarget.health = 0;
            ParentTarget.gameObject.SetActive(false);
            Transform t = ObjectPooling.Ins.SpawnFromPool(Constant.EXPLOSION_TAG, transform.position, Quaternion.identity).transform;
            t.localScale = Vector3.one * 3f;
            ParentTarget.sourceDieSound.Play();

            GameManager_R1S2.Ins.isLose = true;
        }
    }

    
}
