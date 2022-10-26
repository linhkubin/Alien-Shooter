using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Ply_GameUnit
{
    public PoolType poolType;

    void OnEnable()
    {
        Invoke(nameof(DespawnEffect), 0.5f);
    }

    void DespawnEffect()
    {
        Ply_Pool.Ins.Despawn(poolType, this);
    }
}
