using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float speed = 0.5f;

    public Material background;
    private float offset; 
   
    void Update()
    {
        offset += (Time.deltaTime * speed)/ 10f;
        background.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
