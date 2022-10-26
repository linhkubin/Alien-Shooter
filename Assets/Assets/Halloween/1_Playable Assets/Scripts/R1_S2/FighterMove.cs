using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMove : MonoBehaviour
{
    public Rigidbody fighterRb;
    //[HideInInspector] public bool canMove = false;

    private Vector3 mousePosition;
    private Vector2 dir;
    private float speed = 300f;

    [HideInInspector]
    public float _mouseX_Limit = 6f;
    [HideInInspector]
    public float _mouseY_Limit = 10f;
    private Vector3 tmp;

    void Update()
    {
        //if (canMove)
        //{
        if(GameManager_R1S2.Ins.canMoving == true)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tmp = new Vector3(Mathf.Clamp(mousePosition.x, -_mouseX_Limit, _mouseX_Limit), Mathf.Clamp(mousePosition.y, -_mouseY_Limit, _mouseY_Limit), mousePosition.z);
                mousePosition = tmp;

                dir = (mousePosition - transform.position).normalized;
                fighterRb.velocity = new Vector2(dir.x * speed, dir.y * speed);

                //GameManager_GS1.Ins.PauseGame = false;
            }
            else
            {
                fighterRb.velocity = Vector3.zero;
                //GameManager_GS1.Ins.PauseGame = true;
            }
            //}
            //else
            //{
            //    fighterRb.velocity = Vector3.zero;
            //}
        }

    }

    public void SetupMouseLimit(float maxX, float maxY)
    {
        _mouseX_Limit = maxX;
        _mouseY_Limit = maxY;
    }
}
