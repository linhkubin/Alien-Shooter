using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType { Bullet, Enemy, VFX_Spark, VFX_Explore, Enemy_Bullet, Booster }

public class Ply_Pool : Ply_Singleton<Ply_Pool>
{
    public PoolAmount[] poolAmounts;

    [System.Serializable]
    public struct PoolAmount
    {
        public PoolType type;
        public int amount;
        public Ply_GameUnit gameUnit;
    }

    private Dictionary<PoolType, Queue<Ply_GameUnit>> dict = new Dictionary<PoolType, Queue<Ply_GameUnit>>();

    public override void Awake()
    {
        base.Awake();
        OnInit();
    }

    private void OnInit()
    {
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            if (!dict.ContainsKey(poolAmounts[i].type))
            {
                dict[poolAmounts[i].type] = new Queue<Ply_GameUnit>();
            }

            for (int j = 0; j < poolAmounts[i].amount; j++)
            {
                Ply_GameUnit gameUnit = Instantiate(poolAmounts[i].gameUnit);
                gameUnit.gameObject.SetActive(false);
                dict[poolAmounts[i].type].Enqueue(gameUnit);
            }
        }
    }

    public Ply_GameUnit Spawn(PoolType poolType, Vector3 pos, Quaternion rot)
    {
        Ply_GameUnit gameUnit = dict[poolType].Count > 0 ? dict[poolType].Dequeue() : Instantiate(GetPrefab(poolType));

        gameUnit.tf.SetPositionAndRotation(pos, rot);
        gameUnit.gameObject.SetActive(true);

        return gameUnit;
    }
    
    public T Spawn<T>(PoolType poolType, Vector3 pos, Quaternion rot) where T : Ply_GameUnit
    {
        return Spawn(poolType, pos, rot) as T;
    }

    public void Despawn(PoolType poolType, Ply_GameUnit gameUnit)
    {
        gameUnit.gameObject.SetActive(false);
        dict[poolType].Enqueue(gameUnit);
    }


    public Ply_GameUnit GetPrefab(PoolType poolType)
    {
        for (int i = 0; i < poolAmounts.Length; i++)
        {
            if (poolAmounts[i].type == poolType)
            {
                return poolAmounts[i].gameUnit;
            }
        }
        return null;
    }
}
