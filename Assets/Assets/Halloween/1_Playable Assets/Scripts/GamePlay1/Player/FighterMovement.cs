using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMovement : MonoBehaviour
{
    public Rigidbody fighterRb;
    [HideInInspector] public bool canMove = false;

    private Vector3 mousePosition;
    private float speed = 300f;

    [HideInInspector]
    public float _mouseX_Limit = 6f;
    [HideInInspector]
    public float _mouseY_Limit = 10f;
    private Vector3 tmp;

    Camera camera;
    Transform tf;

    private void Awake()
    {
        tf = transform;
        camera = Camera.main;
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
                tmp.x = Mathf.Clamp(mousePosition.x, -_mouseX_Limit, _mouseX_Limit);
                tmp.y = Mathf.Clamp(mousePosition.y, -_mouseY_Limit, _mouseY_Limit);
                tmp.z = mousePosition.z;
                mousePosition = tmp;

                fighterRb.velocity = (mousePosition - tf.position).normalized * speed;
                //GameManager_GS1.Ins.PauseGame = false;
            }
            else
            {
                fighterRb.velocity = Vector3.zero;
                //GameManager_GS1.Ins.PauseGame = true;
            }
        }
        else
        {
            fighterRb.velocity = Vector3.zero;
        }
    }

    public void SetupMouseLimit(float maxX, float maxY)
	{
        _mouseX_Limit = maxX;
        _mouseY_Limit = maxY;
    }
}
 