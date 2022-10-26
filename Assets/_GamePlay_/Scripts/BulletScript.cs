using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector2 moveDirection;
    public float moveSpeed;
    public float timeDestroy;

    private void OnEnable()
    {
        Invoke("Destroy", timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed, Space.Self);
        //transform.Translate(moveDirection * moveSpeed);

        //if (this.transform.position.x >= 7f)
        //{
        //    gameObject.SetActive(false);
        //}

        //if(this.transform.position.x <= -7f)
        //{
        //    gameObject.SetActive(false);
        //}

        //if(this.transform.position.y <= -15f)
        //{
        //    gameObject.SetActive(false);
        //}

        //if(this.transform.position.y >= 15f)
        //{
        //    gameObject.SetActive(false);
        //}

        DestroyOutOfScreen();
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.PLAYER_TAG))
        {
            gameObject.SetActive(false);
            ObjectPooling.Ins.SpawnFromPool(Constant.SPARK_TAG, transform.position, Quaternion.identity);
        }
    }

    public void DestroyOutOfScreen()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (pos.x < 0 || pos.x > 1 || pos.y < 0 || pos.y > 1)
        {
            Destroy();
        }
    }
}
