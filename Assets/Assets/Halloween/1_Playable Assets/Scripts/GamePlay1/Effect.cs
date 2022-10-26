using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    void OnEnable()
    {
        Invoke(nameof(DespawnEffect), 0.5f);
    }

    void DespawnEffect()
    {
        ObjectPooling.Ins.DespawnObject(gameObject.tag, gameObject);
    }
}
