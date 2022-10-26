using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsNoSpine : MonoBehaviour
{
    public Animator wing1, wing2;

    void Start()
    {
        wing1.gameObject.SetActive(false);
        wing2.gameObject.SetActive(false);
    }

    public void ActiveWings()
	{
        wing1.gameObject.SetActive(false);
        wing2.gameObject.SetActive(false);
    }
}
