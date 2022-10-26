using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotRandomSpeedAnim : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetFloat("Speed", Random.Range(0.8f, 1.5f));
    }
}
