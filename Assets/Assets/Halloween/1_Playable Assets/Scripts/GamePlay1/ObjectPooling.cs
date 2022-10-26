using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooling Ins;
    void Awake()
    {
        Ins = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool p in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < p.size; i++)
            {
                GameObject obj = Instantiate(p.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(p.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (poolDictionary != null && poolDictionary.ContainsKey(tag) && poolDictionary[tag].Count > 0)
        {
            GameObject objToSpawn = poolDictionary[tag].Dequeue();

            objToSpawn.SetActive(true);
            objToSpawn.transform.position = pos;
            objToSpawn.transform.rotation = rot;

            return objToSpawn;
        }

        return null;
    }

    public void DespawnObject(string tag, GameObject objToDespawn)
	{
        poolDictionary[tag].Enqueue(objToDespawn);
        objToDespawn.SetActive(false);
    }

}
